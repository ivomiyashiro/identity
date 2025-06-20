using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Commands.Logout.Commands;

public record LogoutCommand : IRequest<Result>;