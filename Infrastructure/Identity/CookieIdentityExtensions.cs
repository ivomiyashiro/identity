using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity;

public static class CookieIdentityExtensions
{
    public static IServiceCollection AddCookieIdentity(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "Identity.Application";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.IsEssential = true;
            options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
            options.Cookie.Path = "/";
            options.SlidingExpiration = true;
        });

        return services;
    }
}