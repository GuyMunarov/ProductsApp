using FluentValidation;
using ProductsApp.API.Products.Models;

namespace ProductsApp.API.Products.Validators;

internal class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage($"{nameof(UpdateProductRequest.Name)} must be at least 3 characters.");
        
        RuleFor(x => x.Color)
            .MinimumLength(3).WithMessage($"{nameof(UpdateProductRequest.Color)} must be at least 3 characters.");
    }
}