namespace API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        // return app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
