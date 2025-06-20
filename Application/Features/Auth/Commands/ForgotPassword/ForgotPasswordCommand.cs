using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommandResponse(string OTP);

public record ForgotPasswordCommand(string Email) : IRequest<Result<ForgotPasswordCommandResponse>>;

