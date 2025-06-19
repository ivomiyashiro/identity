using API.Extensions;
using API.Middleware;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controllers with a general route prefix
builder.Services.AddControllers(options =>
{
    options.UseGeneralRoutePrefix("/api");
});

// Add authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthorization();

// Add infrastructure
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// // Configure CORS for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowReactApp");
}

// Add global exception handling
app.UseExceptionHandling();

// Add base controller routing "api/[controller]"
app.MapControllers();

app.UseHttpsRedirection();

// Serve static files from wwwroot (React build output)
app.UseStaticFiles();

// Use ProblemDetails for error responses
app.UseMiddleware<ExceptionHandlingMiddleware>();

// SPA fallback routing - serve index.html for client-side routes
app.MapFallbackToFile("index.html");

app.Run();
