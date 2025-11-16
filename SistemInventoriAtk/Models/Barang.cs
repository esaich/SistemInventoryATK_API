using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemInventoriAtk.Models
{
    public class Barang
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string KodeBarang { get; set; } = string.Empty; // Unique index dikonfigurasi di DbContext

        [Required]
        [MaxLength(255)]
        public string NamaBarang { get; set; } = string.Empty;

        [Required]
        public int Stok { get; set; } = 0; // Default 0

        [Required]
        [MaxLength(50)]
        public string Satuan { get; set; } = string.Empty;

        // Catatan: Kolom supplier_id dan keterangan dihapus (sesuai migrasi terakhir Anda)

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<PermintaanBarang> PermintaanBarang { get; set; } = new List<PermintaanBarang>();
        public ICollection<BarangMasuk> BarangMasuk { get; set; } = new List<BarangMasuk>();
        public ICollection<BarangKeluar> BarangKeluar { get; set; } = new List<BarangKeluar>();
    }
}