namespace API.Features.Auth.Contracts;

public sealed record VerifyResetOtpResponse(string ResetToken, DateTime ExpiresAt);