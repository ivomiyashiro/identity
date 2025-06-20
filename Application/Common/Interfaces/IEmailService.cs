using SharedKernel.Result;

namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
}
