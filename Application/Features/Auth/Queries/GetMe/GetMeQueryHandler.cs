using Domain.Services;
using MediatR;
using SharedKernel.Result;

namespace Application.Features.Auth.Queries.GetMe;

public class GetMeQueryHandler(IAuthService authService) : IRequestHandler<GetMeQuery, Result<GetMeQueryResponse>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<GetMeQueryResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _authService.GetCurrentUserAsync(request.Identity, cancellationToken);
        
        if (user == null)
        {
            return Result.Failure<GetMeQueryResponse>(
                Error.Unauthorized("Auth.NotAuthenticated", "User is not authenticated"));
        }

        var response = new GetMeQueryResponse(
            user.Id,
            user.FullName,
            user.Email,
            user.CreatedAt);

        return Result.Success(response);
    }
}