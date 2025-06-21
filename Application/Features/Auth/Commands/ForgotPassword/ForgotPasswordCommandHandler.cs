using Application.Common.Interfaces;
using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(IAuthenticationService authenticationService, IEmailService emailService)
    : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var otp = await _authenticationService.ForgotPasswordAsync(request.Email, cancellationToken);

        if (otp == null)
        {
            return Result.Failure(
                Error.NotFound("Auth.UserNotFound", "User not found"));
        }

        var emailResult = await _emailService.SendEmailAsync(request.Email, "Password Reset", $"Your password reset OTP is: {otp}", cancellationToken);
        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        return Result.Success();
    }
}