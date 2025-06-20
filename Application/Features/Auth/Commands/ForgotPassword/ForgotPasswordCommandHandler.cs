using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(IAuthenticationService authenticationService)
    : IRequestHandler<ForgotPasswordCommand, Result<ForgotPasswordCommandResponse>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result<ForgotPasswordCommandResponse>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.ForgotPasswordAsync(request.Email, cancellationToken);

        return Result.Success(new ForgotPasswordCommandResponse("If the email exists, an OTP has been sent."));
    }
}