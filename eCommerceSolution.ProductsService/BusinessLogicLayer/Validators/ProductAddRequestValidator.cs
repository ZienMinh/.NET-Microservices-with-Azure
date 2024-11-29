using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        //ProductName
        RuleFor(temp => temp.ProductName)
            .NotEmpty().WithMessage("Product Name is required");

        //Category
        RuleFor(temp => temp.Category)
            .IsInEnum().WithMessage("Category must be a valid option (Electronics, HomeAndKitchen, SportsAndOutdoors, Fashion)");

        //UnitPrice
        RuleFor(temp => temp.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage($"Unit Price should between 0 to {double.MaxValue}");

        //UnitPrice
        RuleFor(temp => temp.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity should between 0 to {int.MaxValue}");
    }
}
