using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_3.Data;
using Proyecto_3.DTOs.TipoMascotas;
using Proyecto_3.Models;

namespace Proyecto_3.Controllers.AdminControllers
{
    [Route("api/admin/tipomascotas")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class TipoMascotaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoMascotaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTipoMascotas()
        {
            var tipos = await _context.TipoMascotas.ToListAsync();

            return Ok(new
            {
                success = true,
                data = tipos,
                message = "Tipos de mascota obtenidos correctamente."
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoMascota(int id)
        {
            var tipo = await _context.TipoMascotas.FindAsync(id);

            if (tipo == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Tipo de mascota no encontrado."
                });
            }

            return Ok(new
            {
                success = true,
                data = tipo,
                message = "Tipo de mascota encontrado."
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTipoMascota(TipoMascotaCreateDto dto)
        {
            var nombreNormalizado = dto.NombreTipo.Trim().ToLower();

            var existe = await _context.TipoMascotas
                .AnyAsync(t => t.NombreTipo.ToLower() == nombreNormalizado);

            if (existe)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Ya existe un tipo de mascota con ese nombre."
                });
            }

            var tipo = new TipoMascota
            {
                NombreTipo = dto.NombreTipo.Trim()
            };

            _context.TipoMascotas.Add(tipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTipoMascota), new { id = tipo.Id }, new
            {
                success = true,
                data = tipo,
                message = "Tipo de mascota creado correctamente."
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoMascota(int id, TipoMascotaUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El ID de la URL no coincide con el ID del tipo de mascota."
                });
            }

            var tipo = await _context.TipoMascotas.FindAsync(id);

            if (tipo == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Tipo de mascota no encontrado."
                });
            }

            var nombreNormalizado = dto.NombreTipo.Trim().ToLower();

            var existe = await _context.TipoMascotas
                .AnyAsync(t => t.NombreTipo.ToLower() == nombreNormalizado && t.Id != id);

            if (existe)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Ya existe otro tipo de mascota con ese nombre."
                });
            }

            tipo.NombreTipo = dto.NombreTipo.Trim();

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = tipo,
                message = "Tipo de mascota actualizado correctamente."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoMascota(int id)
        {
            var tipo = await _context.TipoMascotas.FindAsync(id);

            if (tipo == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Tipo de mascota no encontrado."
                });
            }

            _context.TipoMascotas.Remove(tipo);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Tipo de mascota eliminado correctamente."
            });
        }
    }
}