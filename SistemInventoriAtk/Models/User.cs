using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemInventoriAtk.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty; // Unique index dikonfigurasi di DbContext

        // Ini hanya untuk API, detail otentikasi (seperti password hash) bisa diabaikan
        [JsonIgnore] // Jangan tampilkan password hash di respons API
        public string Password { get; set; } = string.Empty;

        // Enum role: admin atau divisi (dapat menggunakan string untuk kesederhanaan)
        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "admin";

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties (Relasi)
        public ICollection<PermintaanBarang> PermintaanBarang { get; set; } = new List<PermintaanBarang>();
    }
}