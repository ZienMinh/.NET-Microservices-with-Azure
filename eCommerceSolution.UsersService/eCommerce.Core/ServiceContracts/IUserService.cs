﻿using eCommerce.Core.DTO;

namespace eCommerce.Core.ServiceContracts;

/// <summary>
/// Contract for users service that contains use cases for users
/// </summary>

public interface IUserService
{
    Task<AuthenticationResponse> Login(LoginRequest loginRequest);
    Task<AuthenticationResponse> Register(RegisterRequest registerRequest);
    Task<UserDTO> GetUserByUserID(Guid userID);
}
