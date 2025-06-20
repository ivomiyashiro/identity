using Domain.Entities;

namespace Infrastructure.Features.Identity.Extensions;

public static class AppIdentityUserExtensions
{
    public static User ToDomainUser(this AppIdentityUser identityUser)
    {
        return new User
        {
            Id = Guid.Parse(identityUser.Id),
            FullName = identityUser.FullName,
            Email = identityUser.Email!,
            CreatedAt = DateTime.UtcNow,
            PasswordResetOtp = identityUser.PasswordResetOtp,
            OtpExpiresAt = identityUser.OtpExpiresAt,
            OtpAttempts = identityUser.OtpAttempts
        };
    }

    public static AppIdentityUser ToIdentityUser(this User domainUser, string? id = null)
    {
        return new AppIdentityUser
        {
            Id = id ?? domainUser.Id.ToString(),
            UserName = domainUser.Email,
            Email = domainUser.Email,
            FullName = domainUser.FullName,
            EmailConfirmed = true,
            PasswordResetOtp = domainUser.PasswordResetOtp,
            OtpExpiresAt = domainUser.OtpExpiresAt,
            OtpAttempts = domainUser.OtpAttempts
        };
    }
}