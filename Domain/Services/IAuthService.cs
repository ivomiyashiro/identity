using Domain.Entities;
using System.Security.Principal;

namespace Domain.Services;

public interface IAuthService
{
    Task<bool> CreateUserAccountAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<bool> SignInAsync(User user, CancellationToken cancellationToken = default);
    Task SignOutAsync(CancellationToken cancellationToken = default);
    Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<User?> GetCurrentUserAsync(IIdentity? identity, CancellationToken cancellationToken = default);
    Task<string?> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
    Task<string?> VerifyResetOtpAsync(string email, string otp, CancellationToken cancellationToken = default);
    Task<bool> ResetPasswordAsync(string resetToken, string newPassword, CancellationToken cancellationToken = default);
}