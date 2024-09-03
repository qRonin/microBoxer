using MicroBoxer.GrpcService.Extensions;
using MicroBoxer.GrpcService.Grpc;
using MicroBoxer.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGrpcService<GrpcService>();

app.Run();
