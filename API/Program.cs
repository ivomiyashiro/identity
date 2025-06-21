using API.Extensions;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

// Configure middleware pipeline
app.ConfigureMiddleware();

app.Run();
