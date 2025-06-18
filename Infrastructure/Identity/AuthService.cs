using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager;

    public AuthService(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<string> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        return result.Succeeded ? "User logged in successfully" : "Invalid email or password";
    }

    public async Task<string> Logout()
    {
        await _signInManager.SignOutAsync();
        return "User logged out successfully";
    }

    public async Task<string> Register(string fullName, string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return "User already exists";
        }

        var user = new AppIdentityUser
        {
            UserName = email,
            Email = email,
            FullName = fullName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        return result.Succeeded ? "User created successfully" : "User creation failed";
    }
}
