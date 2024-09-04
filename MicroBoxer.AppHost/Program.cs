using MicroBoxer.AppHost;
using Projects;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

//builder.AddForwardedHeaders();


var postgres = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest");
var rabbitMQ = builder.AddRabbitMQ("eventbus");
var boxesDb = postgres.AddDatabase("boxesservicedb");
var identityDb = postgres.AddDatabase("identitydb");
var apiServiceDb = postgres.AddDatabase("apiservicedb");


var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

var GrpcService = builder.AddProject<Projects.MicroBoxer_GrpcService>("grpcservice")
    .WithReference(rabbitMQ);

var boxesApi = builder.AddProject<Projects.Boxes_API>("BoxesApi")
    .WithReference(rabbitMQ)
    .WithReference(boxesDb)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Identity_API>("IdentityApi")
    .WithReference(identityDb)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Webhook_API>("WebhookApi")
    .WithExternalHttpEndpoints();

var webhooksClient = builder.AddProject<Projects.Webhook_Client>("WebhookClient")
    .WithExternalHttpEndpoints();


var webApp = builder.AddProject<Projects.MicroBoxer_Web>("WebApp")
    .WithExternalHttpEndpoints()
    .WithReference(rabbitMQ)
    .WithReference(boxesApi);
    ;



webApp.WithEnvironment("CallBackUrl", webApp.GetEndpoint(launchProfileName));
webhooksClient.WithEnvironment("CallBackUrl", webhooksClient.GetEndpoint(launchProfileName));





builder.Build().Run();


static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "UBOXER_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}