using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.ResetPassword;

public record ResetPasswordCommandResponse(string Message);

public record ResetPasswordCommand(string ResetToken, string NewPassword) : IRequest<Result<ResetPasswordCommandResponse>>;