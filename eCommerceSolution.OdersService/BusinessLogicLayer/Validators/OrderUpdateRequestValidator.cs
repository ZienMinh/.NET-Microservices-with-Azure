using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        // OrderID
        RuleFor(temp => temp.OrderID)
            .NotEmpty().WithErrorCode("Order ID can't be empty");

        // UserID
        RuleFor(temp => temp.UserID)
            .NotEmpty().WithErrorCode("User ID can't be empty");

        // OrderDate
        RuleFor(temp => temp.OrderDate)
            .NotEmpty().WithErrorCode("Order Date can't be empty");

        // OrderItems
        RuleFor(temp => temp.OrderItems)
            .NotEmpty().WithErrorCode("Order Items can't be empty");
    }
}
