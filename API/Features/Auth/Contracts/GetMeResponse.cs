namespace API.Features.Auth.Contracts;

public sealed record GetMeResponse(
    Guid Id,
    string FullName,
    string Email,
    DateTime CreatedAt); 