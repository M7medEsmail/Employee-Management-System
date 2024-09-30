using Employment_System.Domain.Entities;
using Employment_System.Domain.IServices;
using Employment_System.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ILogger <AuthorizationService>_logger;
        private readonly EmpManageDbContext _empManageContexxt;

        public AuthenticationService(UserManager<AppUser> userManager, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager ,
            IEmailService emailService ,
            ILogger<AuthorizationService> logger ,
            EmpManageDbContext empManageContexxt )
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _emailService = emailService;
            _logger = logger;
            _empManageContexxt = empManageContexxt;
        }

     

        public  async Task< string> GenerateToken(AppUser user)
        {

            //        //var userIdTrimmed = user.Id.Trim();
            var employee =await _empManageContexxt.Employees
            .Where(e => e.AppUserId == user.Id)
            .FirstOrDefaultAsync();

            var employeeId = employee.EmployeeId; // Fallback if employee is null
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("EmployeeId", employeeId.ToString()) // EmployeeId claim with fallback
            };

            // Secret Key

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

            var token = new JwtSecurityToken(
                // registerd claim (get feom app setting)
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDay"])),

                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Login(string Email , string Password)
        {
            var identityUser = await _userManager.FindByEmailAsync(Email);
            if (identityUser == null)
                return false;

            return await _userManager.CheckPasswordAsync(identityUser, Password);
        }

        public async Task<bool> Register(string UserName, string Email, string Password , string requestScheme, string confirmationUrl)
        {
            var User = new AppUser
            {
                Name=UserName,
                UserName = Email.Split('@')[0],
                Email = Email
            };
            var result = await _userManager.CreateAsync(User,Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                var confirmationLink = $"{confirmationUrl}?userId={User.Id}&token={Uri.EscapeDataString(token)}";

                var htmlMessage = $"<h2>Welcome!</h2><p>Please confirm your email by clicking " +
                                  $"<a href='{confirmationLink}'>here</a>.</p>";

                // Check if the email is already confirmed
                var emailConfirmed = await _userManager.IsEmailConfirmedAsync(User);
                if (!emailConfirmed)
                {
                    // Return false or handle it accordingly (e.g., notify the user to confirm their email)
                    _logger.LogInformation($"User {User.UserName} registered, but email not confirmed.");
                    return false; // Email not confirmed
                }

                return true; // Registration succeeded, and email confirmed
            }

            return false; // Registration failed
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return await Task.FromResult(false); // Invalid request
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Task.FromResult(false); // User not found
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }


        public async Task<bool> SendForgotPasswordEmail(string email, string requestScheme, string confirmationUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Return false if the user is not found
                return false;
            }

            // Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create the password reset link
            var resetLink = $"{confirmationUrl}?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            
            try
            {
                // Send the email
                await _emailService.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{resetLink}'>link</a>");

                _logger.LogInformation($"Password reset email sent to '{email}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending password reset email to '{email}': {ex.Message}");
                return false;
            }

            return true;

          
        }

        public async Task<bool> ResetPassword(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with email '{email}' not found.");

                // Return false if the user is not found
                return false;
            }

            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                _logger.LogError($"Failed to reset password for '{email}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return false;
            }

            _logger.LogInformation($"Password successfully reset for '{email}'.");
            return true;
        }


    }
}

