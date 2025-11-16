using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemInventoriAtk.Models
{
    public class PengadaanBarang
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string NamaBarang { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Satuan { get; set; }

        [Required]
        public int JumlahDiajukan { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TanggalPengajuan { get; set; }

        [MaxLength(500)]
        public string? Keterangan { get; set; }

        // Foreign Key ke Supplier
        [Required]
        public int SupplierId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public Supplier Supplier { get; set; } = null!;
    }
}