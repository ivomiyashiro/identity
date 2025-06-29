namespace Application.Features.Auth.Commands.GoogleAuth;

public record GoogleAuthCommandResponse(
    Guid Id,
    string FullName,
    string Email,
    DateTime CreatedAt,
    bool IsNewUser);

