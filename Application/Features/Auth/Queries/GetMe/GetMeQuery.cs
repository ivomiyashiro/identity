using MediatR;
using SharedKernel.Result;
using System.Security.Principal;

namespace Application.Features.Auth.Queries.GetMe;

public record GetMeQuery(IIdentity Identity) : IRequest<Result<GetMeQueryResponse>>;

public record GetMeQueryResponse(
    Guid Id,
    string FullName,
    string Email,
    DateTime CreatedAt);