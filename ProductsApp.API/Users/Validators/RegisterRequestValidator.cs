using FluentValidation;
using ProductsApp.API.Users.Models;

namespace ProductsApp.API.Users.Validators;

internal class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username.Trim())
            .MinimumLength(3).WithMessage($"{nameof(RegisterRequest.Username)} must be at least 3 characters.");
    }
}