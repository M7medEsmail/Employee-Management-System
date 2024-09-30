using System.ComponentModel.DataAnnotations;

namespace Employment_System.Dtos
{
    public class ResetPasswordDto
    {
     
            [Required]
            public string Token { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The password must be at least 6 characters long.", MinimumLength = 6)]
            public string Password { get; set; }
        }
}
