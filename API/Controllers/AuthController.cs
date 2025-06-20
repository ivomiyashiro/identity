using API.Contracts.AuthContracts;
using API.Extensions;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout.Commands;
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
            ? Results.Created($"/api/auth/register", result.Value)
            : result.ToProblemDetails();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpPost("logout")]
    public async Task<IResult> Logout()
    {
        var command = new LogoutCommand();
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }
}