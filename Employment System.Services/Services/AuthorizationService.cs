using Employment_System.Domain.Entities;
using Employment_System.Domain.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Services.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthorizationService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<string>> AddRoleAsync(string[] roles)
        {
            var rolesList = new List<string>();
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    rolesList.Add(role);
                }

            }
            return rolesList;
        }

        public async Task<bool> AddUserRoleAsync(string email, string[] roles)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roleList = new List<string>();
            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    roleList.Add(role);
                }
            }

            if (user != null & roleList.Count == roles.Length)
            {
                var result = await _userManager.AddToRolesAsync(user, roleList);
                return result.Succeeded;
            }
            return false;
        }

            public async Task<List<string>> GetRoleAsync()
            {
            // Use ToListAsync to asynchronously retrieve roles
            var roles = await _roleManager.Roles.ToListAsync();

            // Select the role names and return as a list of strings
            return roles.Select(role => role.Name).ToList();
        }

        public async Task<List<string>> GetUserRoleAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }

        //public async Task<string> SeedingRoles()
        //{
        //    bool IsAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.ADMIN);
        //    bool IsUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.USER);
        //    bool IsMangerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.MANAGER);

        //    if (IsAdminRoleExist && IsUserRoleExist && IsMangerRoleExist)
        //        return " Roles Seeding Already!";

        //    await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.ADMIN));
        //    await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.USER));
        //    await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.MANAGER));
        //    return "Roles Seeding Successfuly";
        //}
    }

}

