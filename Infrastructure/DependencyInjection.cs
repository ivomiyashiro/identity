using Application.Common.Interfaces;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Features.Emails;
using Infrastructure.Features.Identity;
using Infrastructure.Features.Identity.Extensions;
using Infrastructure.Features.Identity.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Register Identity
        services.AddIdentity<AppIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        services.AddCookieIdentity();

        // Register Resend
        services.AddOptions<ResendClientOptions>()
            .Configure<IConfiguration>((opts, config) =>
            {
                opts.ApiToken = config["Resend:ApiKey"] ?? throw new InvalidOperationException("Resend:ApiKey is required");
            });
        services.AddHttpClient<ResendClient>();
        services.AddTransient<IResend, ResendClient>();

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Register services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
