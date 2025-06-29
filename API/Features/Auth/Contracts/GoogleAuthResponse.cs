namespace API.Features.Auth.Contracts;

public sealed record GoogleAuthResponse(
    Guid Id,
    string FullName,
    string Email,
    DateTime CreatedAt,
    bool IsNewUser); 