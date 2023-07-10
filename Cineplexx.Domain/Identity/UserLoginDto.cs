using System.ComponentModel.DataAnnotations;

namespace Cineplexx.Domain.Identity
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Е-пошта е задолжително")]
        [EmailAddress(ErrorMessage = "Неважечка адреса на е-пошта")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Лозинка е задолжително")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
