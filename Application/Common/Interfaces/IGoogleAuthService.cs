using Application.Features.Auth.Commands.GoogleAuth;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IGoogleAuthService
{
    Task<GoogleUserInfoDto?> ValidateGoogleTokenAsync(string googleToken, CancellationToken cancellationToken = default);
    Task<User?> SignInWithGoogleAsync(string email, CancellationToken cancellationToken = default);
}