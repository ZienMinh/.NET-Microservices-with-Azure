using eCommerce.Core.DTO;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    //GET /api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByUserID(Guid id)
    {
        await Task.Delay(10000);
        
        if (id == Guid.Empty)
            return BadRequest("Invalid User ID");

        UserDTO? response = await _userService.GetUserByUserID(id);
        if (response == null)
            return NotFound($"User with ID {id} was not found");

        return Ok(response);
    }
}