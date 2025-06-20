namespace API.Features.Auth.Contracts;

public sealed record RegisterRequest(string FullName, string Email, string Password, string ConfirmPassword);