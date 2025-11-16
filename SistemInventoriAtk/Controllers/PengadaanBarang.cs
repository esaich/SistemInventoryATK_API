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
    public class PengadaanBarangController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PengadaanBarangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================
        // GET: api/pengadaanbarang
        // ====================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.PengadaanBarang
                .Include(p => p.Supplier)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.NamaBarang,
                    p.Satuan,
                    p.JumlahDiajukan,
                    p.TanggalPengajuan,
                    p.Keterangan,
                    Supplier = p.Supplier.NamaSupplier,
                    p.CreatedAt,
                    p.UpdatedAt
                })
                .ToListAsync();

            return Ok(data);
        }

        // ====================================
        // GET: api/pengadaanbarang/{id}
        // ====================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pengadaan = await _context.PengadaanBarang
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pengadaan == null)
                return NotFound(new { message = "Data pengadaan tidak ditemukan." });

            return Ok(new
            {
                pengadaan.Id,
                pengadaan.NamaBarang,
                pengadaan.Satuan,
                pengadaan.JumlahDiajukan,
                pengadaan.TanggalPengajuan,
                pengadaan.Keterangan,
                Supplier = pengadaan.Supplier.NamaSupplier,
                pengadaan.CreatedAt,
                pengadaan.UpdatedAt
            });
        }

        // ====================================
        // POST: api/pengadaanbarang
        // ====================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PengadaanBarang model)
        {
            var supplier = await _context.Suppliers.FindAsync(model.SupplierId);
            if (supplier == null)
                return BadRequest(new { message = "Supplier tidak ditemukan." });

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.PengadaanBarang.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pengadaan barang berhasil ditambahkan." });
        }

        // ====================================
        // PUT: api/pengadaanbarang/{id}
        // ====================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PengadaanBarang model)
        {
            var existing = await _context.PengadaanBarang.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Data pengadaan tidak ditemukan." });

            existing.NamaBarang = model.NamaBarang;
            existing.Satuan = model.Satuan;
            existing.JumlahDiajukan = model.JumlahDiajukan;
            existing.TanggalPengajuan = model.TanggalPengajuan;
            existing.Keterangan = model.Keterangan;
            existing.SupplierId = model.SupplierId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Data pengadaan barang berhasil diperbarui." });
        }

        // ====================================
        // DELETE: api/pengadaanbarang/{id}
        // ====================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pengadaan = await _context.PengadaanBarang.FindAsync(id);
            if (pengadaan == null)
                return NotFound(new { message = "Data pengadaan tidak ditemukan." });

            _context.PengadaanBarang.Remove(pengadaan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data pengadaan barang berhasil dihapus." });
        }
    }
}
