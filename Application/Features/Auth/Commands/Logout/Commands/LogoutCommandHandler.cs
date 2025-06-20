using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.Logout.Commands;

public class LogoutCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<LogoutCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.SignOutAsync(cancellationToken);
        return Result.Success();
    }
}