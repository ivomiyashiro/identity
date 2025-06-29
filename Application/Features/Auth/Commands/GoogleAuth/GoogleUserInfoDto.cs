namespace Application.Features.Auth.Commands.GoogleAuth;

public record GoogleUserInfoDto(
    string Email,
    string Name,
    string GoogleId,
    string? ProfilePictureUrl);