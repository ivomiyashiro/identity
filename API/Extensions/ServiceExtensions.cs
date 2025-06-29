using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Controllers and API
        services.AddControllers();
        services.AddRoutePrefix();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("https://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        // Rate Limiting
        services.AddRateLimitingPolicies();

        // Security
        services.AddSecurityServices();

        return services;
    }

    private static IServiceCollection AddRateLimitingPolicies(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Global rate limiting
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 1000,
                        Window = TimeSpan.FromHours(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 10
                    }));

            // Forgot password: Very restrictive
            options.AddPolicy("ForgotPassword", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: $"forgot_pwd_{httpContext.Connection.RemoteIpAddress}",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromHours(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

            // Login attempts
            options.AddPolicy("Login", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: $"login_{httpContext.Connection.RemoteIpAddress}",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(15),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

            // Google auth attempts
            options.AddPolicy("GoogleAuth", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: $"google_auth_{httpContext.Connection.RemoteIpAddress}",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 15,
                        Window = TimeSpan.FromMinutes(15),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));
        });

        return services;
    }

    private static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        // HTTPS redirection
        services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            options.HttpsPort = 443;
        });

        // HSTS
        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });

        return services;
    }


    private static IServiceCollection AddRoutePrefix(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
        {
            options.Conventions.Add(new RoutePrefixConvention(new RouteAttribute("api")));
        });

        // Configure routing to use lowercase URLs
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });


        return services;
    }

    private class RoutePrefixConvention(IRouteTemplateProvider route) : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix = new(route);

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
            {
                if (selector.AttributeRouteModel != null)
                {
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
                }
                else
                {
                    selector.AttributeRouteModel = _routePrefix;
                }
            }
        }
    }
}