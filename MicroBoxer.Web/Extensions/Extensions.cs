using MicroBoxer.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using EventBusRabbitMQ;
using EventBus.Abstractions;
using MicroBoxer.Web.IntegrationEvents.EventHandlers;
using WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using MicroBoxer.ServiceDefaults;
using MicroBoxer.Web.IntegrationEvents;

namespace MicroBoxer.Web.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();
        builder.AddRabbitMqEventBus("EventBus")
        .AddEventBusSubscriptions();
        builder.Services.AddHttpForwarderWithServiceDiscovery();


        builder.Services.AddSingleton<BoxesNotificationService>();
        builder.Services.AddScoped<LogOutService>();
        builder.Services.AddScoped<UserBoxesState>();
       

        builder.Services.AddHttpClient<BoxesService>(b => b.BaseAddress = new("https://localhost:7046"))
        .AddApiVersion(1.0)
        .AddAuthToken();



    }
    
    public static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        //eventBus.AddSubscription<event, handler>();
        eventBus.AddSubscription <BoxCreatedIntegrationEvent, BoxCreatedIntegrationEventHandler>();
        eventBus.AddSubscription<BoxUpdatedIntegrationEvent, BoxUpdatedIntegrationEventHandler>();
        eventBus.AddSubscription<BoxDeletedIntegrationEvent, BoxDeletedIntegrationEventHandler>();
        eventBus.AddSubscription<BoxContentCreatedIntegrationEvent, BoxContentCreatedIntegrationEventHandler>();
        eventBus.AddSubscription<BoxContentUpdatedIntegrationEvent, BoxContentUpdatedIntegrationEventHandler>();
        eventBus.AddSubscription<BoxContentDeletedIntegrationEvent, BoxContentDeletedIntegrationEventHandler>();
    }
    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        var identityUrl = configuration.GetRequiredValue("IdentityUrl");
        var callBackUrl = configuration.GetRequiredValue("CallBackUrl");
        var sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 2);

        // Add Authentication services
        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(options => options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
        .AddOpenIdConnect(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = identityUrl;
            options.SignedOutRedirectUri = callBackUrl;
            options.ClientId = "webapp";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.RequireHttpsMetadata = false;
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("boxes");
        });

        // Blazor auth services
        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        services.AddCascadingAuthenticationState();
    }

    public static async Task<string?> GetUserIdAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst("sub")?.Value;
    }

    public static async Task<string?> GetUserNameAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst("name")?.Value;
    }
}