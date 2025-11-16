using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemInventoriAtk.Data;
using SistemInventoriAtk.Models;

namespace SistemInventoriAtk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: api/supplier
        // =========================
        [HttpGet]
        [Authorize] // Semua user (admin & divisi) boleh melihat daftar supplier
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _context.Suppliers
                .AsNoTracking()
                .OrderBy(s => s.NamaSupplier)
                .ToListAsync();

            return Ok(suppliers);
        }

        // =========================
        // GET: api/supplier/{id}
        // =========================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound(new { message = "Supplier tidak ditemukan." });

            return Ok(supplier);
        }

        // =========================
        // POST: api/supplier
        // =========================
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")] // hanya admin
        public async Task<IActionResult> Create([FromBody] Supplier model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Cek apakah nama supplier sudah ada
            bool exists = await _context.Suppliers
                .AnyAsync(s => s.NamaSupplier.ToLower() == model.NamaSupplier.ToLower());

            if (exists)
                return Conflict(new { message = "Nama supplier sudah terdaftar." });

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Suppliers.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // =========================
        // PUT: api/supplier/{id}
        // =========================
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")] // hanya admin
        public async Task<IActionResult> Update(int id, [FromBody] Supplier model)
        {
            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Supplier tidak ditemukan." });

            existing.NamaSupplier = model.NamaSupplier;
            existing.Alamat = model.Alamat;
            existing.Telepon = model.Telepon;
            existing.Email = model.Email;
            existing.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Tangani kemungkinan duplikasi nama supplier
                if (await _context.Suppliers.AnyAsync(s => s.NamaSupplier == model.NamaSupplier && s.Id != id))
                {
                    return Conflict(new { message = "Nama supplier sudah digunakan." });
                }

                throw;
            }

            return Ok(existing);
        }

        // =========================
        // DELETE: api/supplier/{id}
        // =========================
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")] // hanya admin
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound(new { message = "Supplier tidak ditemukan." });

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Supplier berhasil dihapus." });
        }
    }
}
