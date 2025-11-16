using System.ComponentModel.DataAnnotations;

namespace SistemInventoriAtk.Models
{
    public class BarangKeluar
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key ke PermintaanBarang (One-to-One)
        [Required]
        public int PermintaanId { get; set; }

        // Foreign Key ke Barang
        [Required]
        public int BarangId { get; set; }

        [Required]
        public int JumlahKeluar { get; set; }

        [Required]
        public DateTime TanggalKeluar { get; set; }

        [MaxLength(500)]
        public string? Keterangan { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public PermintaanBarang Permintaan { get; set; } = null!;
        public Barang Barang { get; set; } = null!;
    }
}