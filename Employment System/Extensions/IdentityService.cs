using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Employment_System.Domain.Entities;
using Employment_System.Repository.Data;

namespace Employment_System.Extensions
{
    public static class IdentityService
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, IdentityRole>(options => // addIdentity used to add interface in create async
            {
                options.SignIn.RequireConfirmedEmail = true; // Require email confirmation

                //options.Password.RequiredLength = 8;
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<EmpManageDbContext>().AddDefaultTokenProviders(); ; // Add ENtity used to implement of create async Function

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]))

                };
            });
            return services;

        }
    }
}
