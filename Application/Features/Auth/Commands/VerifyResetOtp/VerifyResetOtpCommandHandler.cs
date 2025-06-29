using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.VerifyResetOtp;

public class VerifyResetOtpCommandHandler(IAuthService authService)
    : IRequestHandler<VerifyResetOtpCommand, Result<VerifyResetOtpCommandResponse>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<VerifyResetOtpCommandResponse>> Handle(VerifyResetOtpCommand request, CancellationToken cancellationToken)
    {
        var resetToken = await _authService.VerifyResetOtpAsync(request.Email, request.Otp, cancellationToken);

        if (resetToken == null)
        {
            return Result.Failure<VerifyResetOtpCommandResponse>(
                Error.Conflict("Auth.InvalidOtp", "Invalid or expired OTP."));
        }

        var response = new VerifyResetOtpCommandResponse(resetToken, DateTime.UtcNow.AddMinutes(30));
        return Result.Success(response);
    }
}