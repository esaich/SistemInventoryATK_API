using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemInventoriAtk.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key ke Supplier
        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Column(TypeName = "decimal(15, 2)")]
        public decimal TotalHarga { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TanggalBayar { get; set; }

        [MaxLength(500)]
        public string? Keterangan { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public Supplier Supplier { get; set; } = null!;
        // Catatan: BarangMasuk tidak lagi merujuk ke PaymentId (sesuai migrasi)
    }
}