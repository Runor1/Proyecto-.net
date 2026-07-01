using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_3.Data;
using Proyecto_3.DTOs.Mascotas;
using Proyecto_3.Models;

namespace Proyecto_3.Controllers.AdminControllers
{
    [Route("api/admin/mascotas")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class MascotaAdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MascotaAdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/mascotas
        [HttpGet]
        public async Task<IActionResult> GetMascotas()
        {
            var mascotas = await _context.Mascotas
                .Include(m => m.User)
                .Include(m => m.TipoMascota)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = mascotas,
                message = "Mascotas obtenidas correctamente."
            });
        }

        // GET: api/admin/mascotas/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMascota(int id)
        {
            var mascota = await _context.Mascotas
                .Include(m => m.User)
                .Include(m => m.TipoMascota)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mascota == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Mascota no encontrada."
                });
            }

            return Ok(new
            {
                success = true,
                data = mascota,
                message = "Mascota encontrada."
            });
        }

        // POST: api/admin/mascotas
        [HttpPost]
        public async Task<IActionResult> CreateMascota(MascotaCreateDto dto)
        {
            if (dto.UserId.HasValue)
            {
                var usuarioExiste = await _context.Users
                    .AnyAsync(u => u.Id == dto.UserId.Value);

                if (!usuarioExiste)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El usuario no existe."
                    });
                }
            }

            var tipoExiste = await _context.TipoMascotas
                .AnyAsync(t => t.Id == dto.TipoMascotaId);

            if (!tipoExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El tipo de mascota no existe."
                });
            }

            var nombre = dto.Nombre.Trim();

            var mascotaDuplicada = await _context.Mascotas.AnyAsync(m =>
                m.UserId == dto.UserId &&
                m.Nombre.ToLower() == nombre.ToLower());

            if (mascotaDuplicada)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El usuario ya tiene una mascota con ese nombre."
                });
            }

            var mascota = new Mascota
            {
                Nombre = nombre,
                Peso = dto.Peso,
                Altura = dto.Altura,
                Descripcion = dto.Descripcion?.Trim(),
                UserId = dto.UserId,
                TipoMascotaId = dto.TipoMascotaId
            };

            _context.Mascotas.Add(mascota);
            await _context.SaveChangesAsync();

            var mascotaCreada = await _context.Mascotas
                .Include(m => m.User)
                .Include(m => m.TipoMascota)
                .FirstOrDefaultAsync(m => m.Id == mascota.Id);

            return CreatedAtAction(nameof(GetMascota), new { id = mascota.Id }, new
            {
                success = true,
                data = mascotaCreada,
                message = "Mascota creada correctamente."
            });
        }

        // PUT: api/admin/mascotas/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMascota(int id, MascotaUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El ID no coincide."
                });
            }

            var mascota = await _context.Mascotas.FindAsync(id);

            if (mascota == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Mascota no encontrada."
                });
            }

            if (dto.UserId.HasValue)
            {
                var usuarioExiste = await _context.Users
                    .AnyAsync(u => u.Id == dto.UserId.Value);

                if (!usuarioExiste)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El usuario no existe."
                    });
                }
            }

            var tipoExiste = await _context.TipoMascotas
                .AnyAsync(t => t.Id == dto.TipoMascotaId);

            if (!tipoExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El tipo de mascota no existe."
                });
            }

            var nombre = dto.Nombre.Trim();

            var mascotaDuplicada = await _context.Mascotas.AnyAsync(m =>
                m.UserId == dto.UserId &&
                m.Nombre.ToLower() == nombre.ToLower() &&
                m.Id != id);

            if (mascotaDuplicada)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El usuario ya tiene otra mascota con ese nombre."
                });
            }

            mascota.Nombre = nombre;
            mascota.Peso = dto.Peso;
            mascota.Altura = dto.Altura;
            mascota.Descripcion = dto.Descripcion?.Trim();
            mascota.UserId = dto.UserId;
            mascota.TipoMascotaId = dto.TipoMascotaId;

            await _context.SaveChangesAsync();

            var mascotaActualizada = await _context.Mascotas
                .Include(m => m.User)
                .Include(m => m.TipoMascota)
                .FirstOrDefaultAsync(m => m.Id == id);

            return Ok(new
            {
                success = true,
                data = mascotaActualizada,
                message = "Mascota actualizada correctamente."
            });
        }

        // DELETE: api/admin/mascotas/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);

            if (mascota == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Mascota no encontrada."
                });
            }

            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Mascota eliminada correctamente."
            });
        }
    }
}