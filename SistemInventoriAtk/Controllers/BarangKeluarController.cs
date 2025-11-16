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
    public class BarangKeluarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BarangKeluarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================================
        // GET: api/barangkeluar
        // ================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.BarangKeluar
                .Include(b => b.Barang)
                .Include(b => b.Permintaan)
                .OrderByDescending(b => b.TanggalKeluar)
                .Select(b => new
                {
                    b.Id,
                    PermintaanId = b.PermintaanId,
                    Barang = b.Barang.NamaBarang,
                    b.JumlahKeluar,
                    b.TanggalKeluar,
                    b.Keterangan,
                    b.CreatedAt,
                    b.UpdatedAt
                })
                .ToListAsync();

            return Ok(data);
        }

        // ================================
        // POST: api/barangkeluar
        // (Tambah barang keluar secara manual)
        // ================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BarangKeluar model)
        {
            var barang = await _context.Barang.FindAsync(model.BarangId);
            if (barang == null)
                return BadRequest(new { message = "Barang tidak ditemukan." });

            if (barang.Stok < model.JumlahKeluar)
                return BadRequest(new { message = "Stok tidak mencukupi." });

            // Kurangi stok barang
            barang.Stok -= model.JumlahKeluar;

            model.TanggalKeluar = DateTime.UtcNow;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.BarangKeluar.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Barang keluar berhasil dicatat." });
        }

        // ================================
        // DELETE: api/barangkeluar/{id}
        // ================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.BarangKeluar.FindAsync(id);
            if (data == null)
                return NotFound(new { message = "Data tidak ditemukan." });

            _context.BarangKeluar.Remove(data);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data barang keluar berhasil dihapus." });
        }
    }
}
