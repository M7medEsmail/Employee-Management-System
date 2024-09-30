using Employment_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.IServices
{
    public interface IAuthenticationService
    {
        Task<bool> Register(string UserName , string Email , string Password, string requestScheme, string confirmationUrl);

        Task<bool> Login(string Email, string Password);

        Task<bool> ConfirmEmail(string userId, string token);
        Task<string> GenerateToken(AppUser user);
        Task<bool> SendForgotPasswordEmail(string email, string requestScheme, string confirmationUrl);
        Task<bool> ResetPassword(string email, string token, string newPassword);
    }
}
