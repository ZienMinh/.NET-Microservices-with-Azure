﻿namespace BusinessLogicLayer.DTO;

public record UserDTO(
    Guid UserID,
    string? Email,
    string? PersonName,
    string Gender)
{
    public UserDTO() : this(default, default, default, default)
    {
    }
}