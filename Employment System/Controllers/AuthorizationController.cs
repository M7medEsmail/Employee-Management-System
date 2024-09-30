using Employment_System.Domain.IServices;
using Employment_System.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        public AuthorizationController(IAuthorizationService authService)
        {
            _authService = authService;

        }
        private readonly IAuthorizationService _authService;


        [HttpGet("Get Role")]
        public async Task<IActionResult> GetRoles()
        {
            var Roles = await _authService.GetRoleAsync();
            return Ok(Roles);
        }

        [HttpGet("Get User Role")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var userRoles = await _authService.GetUserRoleAsync(email);
            return Ok(userRoles);
        }

        [HttpPost("Add Role")]
        public async Task<IActionResult> AddRoles(string[] roles)
        {
            var Roles = await _authService.AddRoleAsync(roles);
            return Ok(Roles);
        }

        [HttpPost("Add User Role")]
        public async Task<IActionResult> AddUserRoles([FromBody] UserRoleDto userRole)
        {
            var UserRoles = await _authService.AddUserRoleAsync(userRole.Email, userRole.Roles);
            return Ok(UserRoles);
        }
    }
}
