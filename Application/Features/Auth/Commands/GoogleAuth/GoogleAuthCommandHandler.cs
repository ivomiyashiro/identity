using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.GoogleAuth;

public record GoogleAuthCommand(string GoogleToken) : IRequest<Result<GoogleAuthCommandResponse>>; 

public class GoogleAuthCommandHandler(
    IGoogleAuthService googleAuthService,
    IUserRepository userRepository,
    IAuthService authService)
    : IRequestHandler<GoogleAuthCommand, Result<GoogleAuthCommandResponse>>
{
    private readonly IGoogleAuthService _googleAuthService = googleAuthService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthService _authService = authService;

    public async Task<Result<GoogleAuthCommandResponse>> Handle(GoogleAuthCommand request, CancellationToken cancellationToken)
    {
        // Validate Google token and get user info
        var googleUserInfo = await _googleAuthService.ValidateGoogleTokenAsync(request.GoogleToken, cancellationToken);
        if (googleUserInfo == null)
        {
            return Result.Failure<GoogleAuthCommandResponse>(
                Error.Unauthorized("Auth.InvalidGoogleToken", "Invalid Google token"));
        }
    
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(googleUserInfo.Email, cancellationToken);
        bool isNewUser = existingUser == null;

        User user;
        if (isNewUser)
        {
            // Create new user
            user = new User
            {
                Id = Guid.NewGuid(),
                FullName = googleUserInfo.Name,
                Email = googleUserInfo.Email,
                CreatedAt = DateTime.UtcNow
            };

            // Create identity account for Google user (without password)
            var accountCreated = await _authService.CreateUserAccountAsync(user, Guid.NewGuid().ToString(), cancellationToken);
            if (!accountCreated)
            {
                return Result.Failure<GoogleAuthCommandResponse>(
                    Error.Conflict("Auth.AccountCreationFailed", "Failed to create user account"));
            }
        }

        // Sign in the user with Google
        var signedInUser = await _googleAuthService.SignInWithGoogleAsync(googleUserInfo.Email, cancellationToken);
        if (signedInUser == null)
        {
            return Result.Failure<GoogleAuthCommandResponse>(
                Error.Conflict("Auth.GoogleSignInFailed", "Failed to sign in with Google"));
        }

        var response = new GoogleAuthCommandResponse(
            signedInUser.Id,
            signedInUser.FullName,
            signedInUser.Email,
            signedInUser.CreatedAt,
            isNewUser);

        return Result.Success(response);
    }
} 