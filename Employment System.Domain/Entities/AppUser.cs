using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Employment_System.Domain.Entities
{
    public class AppUser :IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? ResetPasswordCode { get; set; }
        public DateTime? ResetCodeExpiry { get; set; }
    }
}
