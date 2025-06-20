using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommandResponse(
    Guid Id,
    string FullName,
    string Email,
    DateTime CreatedAt);


public record LoginCommand(string Email, string Password) : IRequest<Result<LoginCommandResponse>>;