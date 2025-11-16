using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemInventoriAtk.Models
{
    public class BarangMasuk
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key ke Barang
        [Required]
        public int BarangId { get; set; }

        // Foreign Key ke Supplier (sesuai migrasi terakhir)
        public int? SupplierId { get; set; } // Nullable sesuai onDelete('set null')

        [Required]
        public int JumlahMasuk { get; set; }

        [Required]
        [Column(TypeName = "decimal(15, 2)")]
        public decimal HargaSatuan { get; set; }

        [Required]
        public DateTime TanggalMasuk { get; set; } // useCurrent() di Laravel menjadi Required DateTime

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public Barang Barang { get; set; } = null!;
        public Supplier? Supplier { get; set; }
    }
}