using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Text.Json;
using System.Text;
using EventBusRabbitMQ;
using EventBus.Abstractions;
using Swashbuckle.AspNetCore.SwaggerGen;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly.Retry;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Boxes.API.Application.IntegrationEvents.EventHandlers;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.EntityFrameworkCore;
using Boxes.Infrastructure;
using Boxes.API.Infrastructure;
using Boxes.API.Application.IntegrationEvents;
using Boxes.API.Application.Queries;
using IntegrationEventLogEF.Services;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Infrastructure.Repositories;
using Boxes.API.Application.Behaviors;
using MediatR;
using Boxes.Infrastructure.Idempotency;

namespace Boxes.API.Extensions;

public static class Extensions
{

    public static void AddDefaultServices(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMqEventBus("eventbus")
            .AddEventBusSubscriptions();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDbContext<BoxesContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("BoxesServiceDB"));
        });
        builder.EnrichNpgsqlDbContext<BoxesContext>();
        builder.Services.AddMigration<BoxesContext, BoxesContextSeed>();

        builder.Services.AddTransient<IBoxesIntegrationEventService, BoxesIntegrationEventService>();
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<BoxesContext>>();
        builder.Services.AddScoped<IBoxContentQueries, BoxContentQueries>();
        builder.Services.AddScoped<IBoxQueries, BoxQueries>();
        builder.Services.AddScoped<IBoxRepository, BoxRepository>();
        builder.Services.AddScoped<IBoxContentRepository, BoxContentRepository>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
            cfg.AddOpenBehavior(typeof(Transaction<,>));
        });

        builder.Services.AddScoped<IRequestManager, RequestManager>();


    }


    public static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        //eventBus.AddSubscription<IntegrationEvent, IntegrationEventHandler>();

    }

    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
        {
            return app;
        }

        app.UseSwagger();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(setup =>
            {
                /// {
                ///   "OpenApi": {
                ///     "Endpoint: {
                ///         "Name": 
                ///     },
                ///     "Auth": {
                ///         "ClientId": ..,
                ///         "AppName": ..
                ///     }
                ///   }
                /// }

                var pathBase = configuration["PATH_BASE"] ?? string.Empty;
                var authSection = openApiSection.GetSection("Auth");
                var endpointSection = openApiSection.GetRequiredSection("Endpoint");

                foreach (var description in app.DescribeApiVersions())
                {
                    var name = description.GroupName;
                    var url = endpointSection["Url"] ?? $"{pathBase}/swagger/{name}/swagger.json";

                    setup.SwaggerEndpoint(url, name);
                }

                if (authSection.Exists())
                {
                    //setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
                    //setup.OAuthAppName(authSection.GetRequiredValue("AppName"));
                }
            });

            // Add a redirect from the root of the app to the swagger endpoint
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }

        return app;
    }

    public static IHostApplicationBuilder AddDefaultOpenApi(
    this IHostApplicationBuilder builder,
    IApiVersioningBuilder? apiVersioning = default)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        var openApi = configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return builder;
        }

        services.AddEndpointsApiExplorer();

        if (apiVersioning is not null)
        {
            // the default format will just be ApiVersion.ToString(); for example, 1.0.
            // this will format the version as "'v'major[.minor][-status]"
            apiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<OpenApiDefaultValues>());
        }

        return builder;
    }

}
internal sealed class OpenApiDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        // remove any assumed media types not present in the api description
        foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            var response = operation.Responses[responseKey];

            foreach (var contentType in response.Content.Keys)
            {
                if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                {
                    response.Content.Remove(contentType);
                }
            }
        }

        if (operation.Parameters == null)
        {
            return;
        }

        // fix-up parameters with additional information from the api explorer that might
        // not have otherwise been used. this will most often happen for api version parameters
        // which are dynamically added, have no endpoint signature info, nor any xml comments.
        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter.Schema.Default == null &&
                description.DefaultValue != null &&
                description.DefaultValue is not DBNull &&
                description.ModelMetadata is ModelMetadata modelMetadata)
            {
                var json = JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}
internal sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IConfiguration _configuration;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        _provider = provider;
        _configuration = configuration;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        ConfigureAuthorization(options);
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        /// {
        ///   "OpenApi": {
        ///     "Document": {
        ///         "Title": ..
        ///         "Version": ..
        ///         "Description": ..
        ///     }
        ///   }
        /// }
        var openApi = _configuration.GetSection("OpenApi");
        var document = openApi.GetRequiredSection("Document");
        var info = new OpenApiInfo()
        {
            //Title = document.GetRequiredValue("Title"),
            Version = description.ApiVersion.ToString(),
            //Description = BuildDescription(description, document.GetRequiredValue("Description")),
        };

        return info;
    }

    private static string BuildDescription(ApiVersionDescription api, string description)
    {
        var text = new StringBuilder(description);

        if (api.IsDeprecated)
        {
            if (text.Length > 0)
            {
                if (text[^1] != '.')
                {
                    text.Append('.');
                }

                text.Append(' ');
            }

            text.Append("This API version has been deprecated.");
        }

        if (api.SunsetPolicy is { } policy)
        {
            if (policy.Date is { } when)
            {
                if (text.Length > 0)
                {
                    text.Append(' ');
                }

                text.Append("The API will be sunset on ")
                    .Append(when.Date.ToShortDateString())
                    .Append('.');
            }

            if (policy.HasLinks)
            {
                text.AppendLine();

                var rendered = false;

                foreach (var link in policy.Links.Where(l => l.Type == "text/html"))
                {
                    if (!rendered)
                    {
                        text.Append("<h4>Links</h4><ul>");
                        rendered = true;
                    }

                    text.Append("<li><a href=\"");
                    text.Append(link.LinkTarget.OriginalString);
                    text.Append("\">");
                    text.Append(
                        StringSegment.IsNullOrEmpty(link.Title)
                        ? link.LinkTarget.OriginalString
                        : link.Title.ToString());
                    text.Append("</a></li>");
                }

                if (rendered)
                {
                    text.Append("</ul>");
                }
            }
        }

        return text.ToString();
    }

    private void ConfigureAuthorization(SwaggerGenOptions options)
    {
        var identitySection = _configuration.GetSection("Identity");

        if (!identitySection.Exists())
        {
            // No identity section, so no authentication open api definition
            return;
        }

        // {
        //   "Identity": {
        //     "Url": "http://identity",
        //     "Scopes": {
        //         "basket": "Basket API"
        //      }
        //    }
        // }
        /*
        var identityUrlExternal = identitySection.GetRequiredValue("Url");
        var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows()
            {
                // TODO: Change this to use Authorization Code flow with PKCE
                Implicit = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
                    TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
                    Scopes = scopes,
                }
            }
        });

        options.OperationFilter<AuthorizeCheckOperationFilter>([scopes.Keys.ToArray()]);
        */
    }
}
