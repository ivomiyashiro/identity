using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.VerifyResetOtp;

public record VerifyResetOtpCommandResponse(
    string ResetToken,
    DateTime ExpiresAt);

public record VerifyResetOtpCommand(string Email, string Otp) : IRequest<Result<VerifyResetOtpCommandResponse>>;