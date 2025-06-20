namespace API.Features.Auth.Contracts;

public sealed record VerifyResetOtpRequest(string Email, string Otp);