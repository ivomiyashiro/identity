using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Features.Identity;
using Infrastructure.Features.Identity.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistance.Repositories;

public class UserRepository(UserManager<AppIdentityUser> userManager) : IUserRepository
{
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);
        return identityUser?.ToDomainUser();
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());
        return identityUser?.ToDomainUser();
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);
        return identityUser != null;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (identityUser != null)
        {
            identityUser.FullName = user.FullName;
            identityUser.Email = user.Email;
            identityUser.UserName = user.Email;
            identityUser.PasswordResetOtp = user.PasswordResetOtp;
            identityUser.OtpExpiresAt = user.OtpExpiresAt;
            identityUser.OtpAttempts = user.OtpAttempts;

            await _userManager.UpdateAsync(identityUser);
        }
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (identityUser != null)
        {
            await _userManager.DeleteAsync(identityUser);
        }
    }
}