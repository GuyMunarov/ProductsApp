using FluentValidation;
using ProductsApp.API.Products.Models;

namespace ProductsApp.API.Products.Validators;

internal class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage($"{nameof(UpdateProductRequest.Name)} must be at least 3 characters.");
        
        RuleFor(x => x.Color)
            .MinimumLength(3).WithMessage($"{nameof(UpdateProductRequest.Color)} must be at least 3 characters.");
    }
}