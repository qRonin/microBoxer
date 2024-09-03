using Boxes.API.Apis;
using Boxes.API.Application.Commands.Box;
using Boxes.API.Extensions;
using MicroBoxer.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultServices();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();

var boxesApi = app.NewVersionedApi("BoxesApi");

boxesApi.MapBoxesApiV1();
//.RequireAuthorization();

app.UseDefaultOpenApi();
app.Run();
