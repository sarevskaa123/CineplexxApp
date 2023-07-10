using System.ComponentModel.DataAnnotations;

namespace Cineplexx.Domain.Identity
{
    public class UserRegistrationDto
    {
        [EmailAddress(ErrorMessage = "Неважечка адреса на е-пошта")]
        [Required(ErrorMessage = "Е-пошта е задолжително")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Лозинка е задолжително")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Потврдење на лозинка е задолжително")]
        [Compare("Password", ErrorMessage = "Тие не се совпаѓаат")]
        public string ConfirmPassword { get; set; }
    }
}
