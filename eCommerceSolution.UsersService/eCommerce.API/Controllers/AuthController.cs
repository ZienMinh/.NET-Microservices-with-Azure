using eCommerce.Core.DTO;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        // Check for invalid registerRequest
        if (registerRequest == null)
            return BadRequest("Invalid registration data");

        // Call the UserService to handle registration
        AuthenticationResponse? authenticationResponse = await _userService.Register(registerRequest);

        if (authenticationResponse == null || authenticationResponse.Success == false)
            return BadRequest(authenticationResponse);

        return Ok(authenticationResponse);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        // Check for invalid loginRequest
        if (loginRequest == null)
            return BadRequest("Invalid login data");

        AuthenticationResponse? authenticationResponse = await _userService.Login(loginRequest);
        if (authenticationResponse == null || authenticationResponse.Success == false)
            return Unauthorized(authenticationResponse);

        return Ok(authenticationResponse);
    }
}
