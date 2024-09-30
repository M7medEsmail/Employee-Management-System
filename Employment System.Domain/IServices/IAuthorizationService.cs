using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.IServices
{
    public interface IAuthorizationService
    {
        Task<List<string>> GetRoleAsync();
        Task<List<string>> GetUserRoleAsync(string email);
        Task<bool> AddUserRoleAsync(string email, string[] roles);
        Task<List<string>> AddRoleAsync(string[] roles);

    }
}
