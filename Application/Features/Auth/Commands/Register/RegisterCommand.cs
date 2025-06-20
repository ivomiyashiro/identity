using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.Register;

public record RegisterCommandResponse(
    Guid Id,
    string FullName,
    string Email,
    DateTime CreatedAt);

public record RegisterCommand(
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<Result<RegisterCommandResponse>>;