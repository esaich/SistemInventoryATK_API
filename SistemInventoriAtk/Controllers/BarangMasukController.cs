using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemInventoriAtk.Data;
using SistemInventoriAtk.Models;

namespace SistemInventoriAtk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class BarangMasukController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BarangMasukController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================================================
        // GET: api/barangmasuk
        // ===================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.BarangMasuk
                .Include(b => b.Barang)
                .Include(b => b.Supplier)
                .OrderByDescending(b => b.TanggalMasuk)
                .Select(b => new
                {
                    b.Id,
                    Barang = b.Barang.NamaBarang,
                    Supplier = b.Supplier != null ? b.Supplier.NamaSupplier : "-",
                    b.JumlahMasuk,
                    b.HargaSatuan,
                    TotalHarga = b.JumlahMasuk * b.HargaSatuan,
                    b.TanggalMasuk,
                    b.CreatedAt,
                    b.UpdatedAt
                })
                .ToListAsync();

            return Ok(data);
        }

        // ===================================================
        // POST: api/barangmasuk
        // ===================================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BarangMasuk model)
        {
            var barang = await _context.Barang.FindAsync(model.BarangId);
            if (barang == null)
                return BadRequest(new { message = "Barang tidak ditemukan." });

            // Supplier boleh null, jadi tidak perlu validasi wajib ada
            if (model.SupplierId.HasValue)
            {
                var supplier = await _context.Suppliers.FindAsync(model.SupplierId.Value);
                if (supplier == null)
                    return BadRequest(new { message = "Supplier tidak ditemukan." });
            }

            // Tambahkan stok barang
            barang.Stok += model.JumlahMasuk;

            model.TanggalMasuk = DateTime.UtcNow;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.BarangMasuk.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Barang masuk berhasil dicatat dan stok diperbarui." });
        }

        // ===================================================
        // PUT: api/barangmasuk/{id}
        // ===================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BarangMasuk model)
        {
            var barangMasuk = await _context.BarangMasuk.FindAsync(id);
            if (barangMasuk == null)
                return NotFound(new { message = "Data barang masuk tidak ditemukan." });

            var barang = await _context.Barang.FindAsync(barangMasuk.BarangId);
            if (barang == null)
                return BadRequest(new { message = "Barang tidak ditemukan." });

            // Hitung selisih stok
            int selisih = model.JumlahMasuk - barangMasuk.JumlahMasuk;
            barang.Stok += selisih;

            // Update data
            barangMasuk.BarangId = model.BarangId;
            barangMasuk.SupplierId = model.SupplierId;
            barangMasuk.JumlahMasuk = model.JumlahMasuk;
            barangMasuk.HargaSatuan = model.HargaSatuan;
            barangMasuk.TanggalMasuk = model.TanggalMasuk;
            barangMasuk.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Data barang masuk berhasil diperbarui dan stok disesuaikan." });
        }

        // ===================================================
        // DELETE: api/barangmasuk/{id}
        // ===================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var barangMasuk = await _context.BarangMasuk.FindAsync(id);
            if (barangMasuk == null)
                return NotFound(new { message = "Data barang masuk tidak ditemukan." });

            var barang = await _context.Barang.FindAsync(barangMasuk.BarangId);
            if (barang == null)
                return BadRequest(new { message = "Barang tidak ditemukan." });

            // Kurangi stok sesuai jumlah masuk yang dihapus
            barang.Stok -= barangMasuk.JumlahMasuk;

            _context.BarangMasuk.Remove(barangMasuk);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data barang masuk berhasil dihapus dan stok diperbarui." });
        }
    }
}
