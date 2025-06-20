using Application.Common.Interfaces;
using Resend;
using SharedKernel.Result;

namespace Infrastructure.Features.Emails;

public class EmailService(IResend resend) : IEmailService
{
    private readonly IResend _resend = resend;


    public async Task<Result> SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new EmailMessage
            {
                From = "onboarding@resend.dev",
                To = [toEmail],
                Subject = subject,
                HtmlBody = body
            };

            var response = await _resend.EmailSendAsync(message, cancellationToken);

            return response.Success
                ? Result.Success()
                : Result.Failure(Error.Conflict("Email.NotSent", response.Exception?.Message ?? "Unknown error"));
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Conflict("Email.Exception", ex.Message ?? "Unknown error"));
        }
    }
}