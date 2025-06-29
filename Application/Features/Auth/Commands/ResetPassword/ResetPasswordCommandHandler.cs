using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IAuthService authService)
    : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordCommandResponse>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<ResetPasswordCommandResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var success = await _authService.ResetPasswordAsync(request.ResetToken, request.NewPassword, cancellationToken);

        if (!success)
        {
            return Result.Failure<ResetPasswordCommandResponse>(
                Error.Unauthorized("Auth.InvalidResetToken", "Invalid or expired reset token."));
        }

        return Result.Success(new ResetPasswordCommandResponse("Password reset successfully."));
    }
}