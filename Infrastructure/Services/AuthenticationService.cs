using Domain.Entities;
using Domain.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class AuthenticationService(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager) : IAuthenticationService
{
    private readonly UserManager<AppIdentityUser> _userManager = userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager = signInManager;

    public async Task<bool> CreateUserAccountAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var identityUser = user.ToIdentityUser();
        var result = await _userManager.CreateAsync(identityUser, password);

        return result.Succeeded;
    }

    public async Task<bool> SignInAsync(User user, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByEmailAsync(user.Email);
        if (identityUser == null) return false;

        await _signInManager.SignInAsync(identityUser, isPersistent: false);
        return true;
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        return result.Succeeded;
    }
}