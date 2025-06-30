using API.Common;
using API.Extensions;
using API.Features.Auth.Contracts;
using Application.Features.Auth.Commands.ForgotPassword;
using Application.Features.Auth.Commands.GoogleAuth;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout.Commands;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.ResetPassword;
using Application.Features.Auth.Commands.VerifyResetOtp;
using Application.Features.Auth.Queries.GetMe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Features.Auth;

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
    [EnableRateLimiting("Login")]
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

    [AllowAnonymous]
    [EnableRateLimiting("ForgotPassword")]
    [HttpPost("forgot-password")]
    public async Task<IResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand(request.Email);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }

    [AllowAnonymous]
    [HttpPost("verify-reset-otp")]
    public async Task<IResult> VerifyResetOtp([FromBody] VerifyResetOtpRequest request)
    {
        var command = new VerifyResetOtpCommand(request.Email, request.Otp);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var command = new ResetPasswordCommand(request.ResetToken, request.NewPassword);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [AllowAnonymous]
    [EnableRateLimiting("GoogleAuth")]
    [HttpPost("google")]
    public async Task<IResult> GoogleAuth([FromBody] GoogleAuthRequest request)
    {
        var command = new GoogleAuthCommand(request.GoogleToken);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpGet("me")]
    public async Task<IResult> GetCurrentUser()
    {
        var query = new GetMeQuery(User.Identity!);
        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }
}