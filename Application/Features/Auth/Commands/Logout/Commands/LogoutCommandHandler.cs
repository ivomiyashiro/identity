using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.Logout.Commands;

public class LogoutCommandHandler(IAuthService authService)
    : IRequestHandler<LogoutCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authService.SignOutAsync(cancellationToken);
        return Result.Success();
    }
}