var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest");
var rabbitMQ = builder.AddRabbitMQ("eventbus");
var boxesDb = postgres.AddDatabase("boxesservicedb");
var identityDb = postgres.AddDatabase("identitydb");
var apiServiceDb = postgres.AddDatabase("apiservicedb");


var GrpcService = builder.AddProject<Projects.MicroBoxer_GrpcService>("grpcservice")
    .WithReference(rabbitMQ);

builder.AddProject<Projects.Boxes_API>("BoxesApi")
    .WithReference(rabbitMQ)
    .WithReference(boxesDb)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Identity_API>("IdentityApi")
    .WithReference(identityDb)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Webhook_API>("WebhookApi")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Webhook_Client>("WebhookClient")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.MicroBoxer_Web>("WebApp")
    .WithExternalHttpEndpoints()
    .WithReference(rabbitMQ)
    ;






builder.Build().Run();
