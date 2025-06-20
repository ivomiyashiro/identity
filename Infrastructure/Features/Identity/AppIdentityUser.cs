using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Features.Identity;

public class AppIdentityUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    // Password reset fields
    public string? PasswordResetOtp { get; set; }
    public DateTime? OtpExpiresAt { get; set; }
    public int OtpAttempts { get; set; }
}