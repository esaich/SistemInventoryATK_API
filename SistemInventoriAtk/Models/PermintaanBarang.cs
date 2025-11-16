using System.ComponentModel.DataAnnotations;

namespace SistemInventoriAtk.Models
{
    public class PermintaanBarang
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key ke User
        [Required]
        public int UserId { get; set; }

        // Foreign Key ke Barang
        [Required]
        public int BarangId { get; set; }

        [Required]
        public int Jumlah { get; set; }

        // Enum status: pending, disetujui, ditolak
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "pending";

        [MaxLength(500)]
        public string? Alasan { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;
        public Barang Barang { get; set; } = null!;
        public BarangKeluar? BarangKeluar { get; set; } // One-to-One/Zero relasi
    }
}