using Domain.Entities;

namespace Domain.Services;

public interface IAuthenticationService
{
    Task<bool> CreateUserAccountAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<bool> SignInAsync(User user, CancellationToken cancellationToken = default);
    Task SignOutAsync(CancellationToken cancellationToken = default);
    Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
}