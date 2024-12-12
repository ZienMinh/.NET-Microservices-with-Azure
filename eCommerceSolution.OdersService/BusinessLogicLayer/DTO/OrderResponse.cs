﻿namespace BusinessLogicLayer.DTO;

public record OrderResponse(
    Guid OrderID,
    Guid UserID,
    string? Email,
    string? UserPersonName,
    decimal TotalBill,
    DateTime OrderDate,
    List<OrderItemResponse> OrderItems)
{
    public OrderResponse() : this(default, default, default,
        default, default, default, default)
    {
    }
}