using AutoMapper;
using Employment_System.Domain.Entities;
using Employment_System.Domain.IServices;
using Employment_System.Dtos;
using Employment_System.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Employment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly EmpManageDbContext _empManageDbContext;
        private readonly UserManager<AppUser> _userManager;

        public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper,
            EmpManageDbContext empManageDbContext, UserManager<AppUser> userManager)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _empManageDbContext = empManageDbContext;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto user)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var confirmationUrl = Url.Action("ConfirmEmail", "Authentication", null, Request.Scheme);
            var isRegistered = await _authenticationService.Register(user.UserName, user.Email, user.Password, Request.Scheme, confirmationUrl);

            if (isRegistered)
            {
                return Ok(new { Message = "Registration successful! Please check your email to confirm." });
            }

            return BadRequest("Registration failed.");
        }
        [HttpGet("Confirm Password")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var isConfirmed = await _authenticationService.ConfirmEmail(userId, token);

            if (isConfirmed)
            {
                return Ok(new { Message = "Email confirmed successfully!" });
            }

            return BadRequest("Error confirming your email.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginUserDto)
        {
           var x= _empManageDbContext.Employees.ToList();
            if (await _authenticationService.Login(loginUserDto.Email ,loginUserDto.Password))
            {

                var user =await _userManager.FindByEmailAsync(loginUserDto.Email);
                
                return Ok(new UserDto()
                {
                    DisplayName = loginUserDto.Email,
                    Token =await _authenticationService.GenerateToken(user),
                    
                });
            }
            return BadRequest();
        }


        [HttpPost("Forgot Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var confirmationUrl = Url.Action("ResetPassword", "Authentication", null, Request.Scheme);
            
            var result = await _authenticationService.SendForgotPasswordEmail(model.Email ,Request.Scheme , confirmationUrl);

            if (!result)
            {
                // Return Ok even if the email is not found to prevent user enumeration
                return Ok("If this email exists, a password reset link has been sent.");
            }

            return Ok("Password reset link sent.");
        }

        [HttpPost("Reset Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.ResetPassword(model.Email, model.Token, model.Password);

            if (!result)
            {
                return BadRequest("Invalid token or email.");
            }

            return Ok("Password has been reset successfully.");
        }


    }
}
