namespace API.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        // Development-specific middleware
        if (app.Environment.IsDevelopment())
        {
            app.ConfigureDevelopmentMiddleware();
        }
        else
        {
            app.ConfigureProductionMiddleware();
        }

        // Security middleware (always)
        app.ConfigureSecurityMiddleware();

        // Core middleware
        app.ConfigureCoreMiddleware();

        return app;
    }

    private static WebApplication ConfigureDevelopmentMiddleware(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();

        return app;
    }

    private static WebApplication ConfigureProductionMiddleware(this WebApplication app)
    {
        app.UseHsts();
        app.UseExceptionHandler("/Error");

        return app;
    }

    private static WebApplication ConfigureSecurityMiddleware(this WebApplication app)
    {
        // HTTPS enforcement
        app.UseHttpsRedirection();

        // Security headers
        app.UseSecurityHeaders();

        // Rate limiting
        app.UseRateLimiter();

        return app;
    }

    private static WebApplication ConfigureCoreMiddleware(this WebApplication app)
    {
        // Static files
        app.UseStaticFiles();

        // Routing
        app.UseRouting();

        // CORS
        app.UseCors("AllowFrontend");

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Controllers
        app.MapControllers();

        // Fallback for SPA
        app.MapFallbackToFile("index.html");

        return app;
    }

    private static WebApplication UseSecurityHeaders(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            // Security headers for all requests
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

            // Extra security for auth endpoints
            if (context.Request.Path.StartsWithSegments("/auth"))
            {
                context.Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate");
                context.Response.Headers.Append("Pragma", "no-cache");
            }

            await next();
        });

        return app;
    }
}