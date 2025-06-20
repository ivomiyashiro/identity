using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordCommandResponse>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result<ResetPasswordCommandResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var success = await _authenticationService.ResetPasswordAsync(request.ResetToken, request.NewPassword, cancellationToken);

        if (!success)
        {
            return Result.Failure<ResetPasswordCommandResponse>(
                Error.Validation("Auth.InvalidResetToken", "Invalid or expired reset token."));
        }

        return Result.Success(new ResetPasswordCommandResponse("Password reset successfully."));
    }
}