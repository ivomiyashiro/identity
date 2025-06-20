using Domain.Repositories;
using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
    : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var isValid = await _authenticationService.ValidateCredentialsAsync(request.Email, request.Password, cancellationToken);
        if (!isValid)
        {
            return Result.Failure<LoginCommandResponse>(Error.Conflict("Auth.InvalidCredentials", "Invalid email or password"));
        }

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            return Result.Failure<LoginCommandResponse>(Error.Conflict("Auth.UserNotFound", "User not found"));
        }

        return Result.Success(new LoginCommandResponse(user.Id, user.FullName, user.Email, user.CreatedAt));
    }
}