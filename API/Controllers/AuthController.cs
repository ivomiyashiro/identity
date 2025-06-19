using API.Contracts.AuthContracts;
using API.Extensions;
using Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator) : BaseController(mediator)
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FullName,
            request.Email,
            request.Password,
            request.ConfirmPassword);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Results.Created($"/api/auth/register", result)
            : result.ToProblemDetails();
    }
}