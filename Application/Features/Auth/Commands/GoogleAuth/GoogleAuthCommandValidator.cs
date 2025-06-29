using FluentValidation;

namespace Application.Features.Auth.Commands.GoogleAuth;

public class GoogleAuthCommandValidator : AbstractValidator<GoogleAuthCommand>
{
    public GoogleAuthCommandValidator()
    {
        RuleFor(x => x.GoogleToken)
            .NotEmpty()
            .WithMessage("Google token is required");
    }
} 