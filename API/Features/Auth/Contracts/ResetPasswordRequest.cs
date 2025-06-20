namespace API.Features.Auth.Contracts;

public sealed record ResetPasswordRequest(string ResetToken, string NewPassword);