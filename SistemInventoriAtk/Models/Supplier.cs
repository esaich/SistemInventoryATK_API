using System.ComponentModel.DataAnnotations;

namespace SistemInventoriAtk.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string NamaSupplier { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Alamat { get; set; }

        [Required]
        [MaxLength(50)]
        public string Telepon { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<PengadaanBarang> PengadaanBarang { get; set; } = new List<PengadaanBarang>();

        // Catatan: Relasi ke Barang dihilangkan (sesuai migrasi terakhir Anda)
    }
}