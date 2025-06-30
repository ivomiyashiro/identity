using Application.Common.Interfaces;
using Application.Features.Auth.Commands.GoogleAuth;
using Domain.Entities;
using Domain.Repositories;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Features.Identity.Services;

public class GoogleAuthService(
    IConfiguration configuration,
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    IUserRepository userRepository) : IGoogleAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager = signInManager;
    private readonly IUserRepository _userRepository = userRepository;

     public async Task<GoogleUserInfoDto?> ValidateGoogleTokenAsync(string googleToken, CancellationToken cancellationToken = default)
    {
        var clientId = _configuration["Google:ClientId"];
        if (string.IsNullOrEmpty(clientId))
            return null;

        var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [clientId]
        });

        return new GoogleUserInfoDto(
            Email: payload.Email,
            Name: payload.Name,
            GoogleId: payload.Subject,
            ProfilePictureUrl: payload.Picture);
    }

    public async Task<User?> SignInWithGoogleAsync(string email, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (identityUser == null)
        {
            return null;
        }

        // Sign in the user
        await _signInManager.SignInAsync(identityUser, isPersistent: false);

        // Return the domain user
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return user;
    }
}