using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
    : IRequestHandler<RegisterCommand, Result<RegisterCommandResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email, cancellationToken))
        {
            return Result.Failure<RegisterCommandResponse>(
                Error.Conflict("Auth.UserExists", "A user with this email already exists"));
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        var accountCreated = await _authenticationService.CreateUserAccountAsync(user, request.Password, cancellationToken);
        if (!accountCreated)
        {
            return Result.Failure<RegisterCommandResponse>(
                Error.Conflict("Auth.AccountCreationFailed", "Failed to create user account"));
        }

        var response = new RegisterCommandResponse(
            user.Id,
            user.FullName,
            user.Email,
            user.CreatedAt);

        return Result.Success(response);
    }
}