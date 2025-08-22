using FastEndpoints;
using FluentValidation;
using StockHarbor.API.Models.Products.Request;

namespace StockHarbor.API.Validators.Product;

public class CreateProductValidator : Validator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters.");
        RuleFor(x => x.ProductType)
            .NotEmpty().WithMessage("Product type is required.")
            .IsInEnum().WithMessage("Invalid Product type");
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Product status is required.")
            .IsInEnum().WithMessage("Invalid product status.");
    }
}
