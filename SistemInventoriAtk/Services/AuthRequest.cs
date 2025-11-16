using System.ComponentModel.DataAnnotations;

namespace SistemInventoriAtk.Services
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Email Wajib Diisi")]
        [EmailAddress(ErrorMessage = "Format Email Tidak Valid")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password Wajib Diisi")]
        public string Password { get; set; } = string.Empty;
    }
}
