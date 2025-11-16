using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemInventoriAtk.Data;
using SistemInventoriAtk.Models;

namespace SistemInventoriAtk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarangController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BarangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: api/barang
        // =========================
        [HttpGet]
        [Authorize] // Semua user (admin & divisi) bisa melihat daftar barang
        public async Task<IActionResult> GetAll()
        {
            var barangList = await _context.Barang
                .AsNoTracking()
                .OrderBy(b => b.NamaBarang)
                .ToListAsync();

            return Ok(barangList);
        }

        // =========================
        // GET: api/barang/{id}
        // =========================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var barang = await _context.Barang
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (barang == null)
                return NotFound(new { message = "Barang tidak ditemukan." });

            return Ok(barang);
        }

        // =========================
        // POST: api/barang
        // =========================
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")] // hanya admin
        public async Task<IActionResult> Create([FromBody] Barang model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Cek apakah kode barang sudah ada
            bool kodeExists = await _context.Barang
                .AnyAsync(b => b.KodeBarang == model.KodeBarang);

            if (kodeExists)
                return Conflict(new { message = "Kode barang sudah terdaftar." });

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Barang.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // =========================
        // PUT: api/barang/{id}
        // =========================
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")] // hanya admin
        public async Task<IActionResult> Update(int id, [FromBody] Barang model)
        {
            var existing = await _context.Barang.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Barang tidak ditemukan." });

            // Update field
            existing.KodeBarang = model.KodeBarang;
            existing.NamaBarang = model.NamaBarang;
            existing.Satuan = model.Satuan;
            existing.Stok = model.Stok;
            existing.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Tangani kode barang duplikat
                if (await _context.Barang.AnyAsync(b => b.KodeBarang == model.KodeBarang && b.Id != id))
                {
                    return Conflict(new { message = "Kode barang sudah digunakan." });
                }

                throw;
            }

            return Ok(existing);
        }

        // =========================
        // DELETE: api/barang/{id}
        // =========================
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")] // hanya admin
        public async Task<IActionResult> Delete(int id)
        {
            var barang = await _context.Barang.FindAsync(id);
            if (barang == null)
                return NotFound(new { message = "Barang tidak ditemukan." });

            _context.Barang.Remove(barang);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Barang berhasil dihapus." });
        }
    }
}
