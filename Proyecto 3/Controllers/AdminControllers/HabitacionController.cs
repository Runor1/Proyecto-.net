using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_3.Data;
using Proyecto_3.DTOs.Habitaciones;
using Proyecto_3.Models;

namespace Proyecto_3.Controllers.AdminControllers
{
    [Route("api/admin/habitaciones")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class HabitacionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitacionController(AppDbContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetHabitaciones()
        {
            var habitaciones = await _context.Habitaciones
                .Include(h => h.TipoMascota)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = habitaciones,
                message = "Habitaciones obtenidas correctamente."
            });
        }

        // GET por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabitacion(int id)
        {
            var habitacion = await _context.Habitaciones
                .Include(h => h.TipoMascota)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (habitacion == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Habitación no encontrada."
                });
            }

            return Ok(new
            {
                success = true,
                data = habitacion,
                message = "Habitación encontrada."
            });
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateHabitacion(HabitacionCreateDto dto)
        {
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

            var descripcion = dto.Descripcion.Trim();

            var existe = await _context.Habitaciones
                .AnyAsync(h => h.Descripcion.ToLower() == descripcion.ToLower());

            if (existe)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Ya existe una habitación con esa descripción."
                });
            }

            var habitacion = new Habitacion
            {
                Descripcion = descripcion,
                TipoMascotaId = dto.TipoMascotaId
            };

            _context.Habitaciones.Add(habitacion);
            await _context.SaveChangesAsync();

            habitacion = await _context.Habitaciones
                .Include(h => h.TipoMascota)
                .FirstAsync(h => h.Id == habitacion.Id);

            return CreatedAtAction(nameof(GetHabitacion), new { id = habitacion.Id }, new
            {
                success = true,
                data = habitacion,
                message = "Habitación creada correctamente."
            });
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabitacion(int id, HabitacionUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El ID no coincide."
                });
            }

            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Habitación no encontrada."
                });
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

            var descripcion = dto.Descripcion.Trim();

            var existe = await _context.Habitaciones
                .AnyAsync(h =>
                    h.Descripcion.ToLower() == descripcion.ToLower()
                    && h.Id != id);

            if (existe)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Ya existe otra habitación con esa descripción."
                });
            }

            habitacion.Descripcion = descripcion;
            habitacion.TipoMascotaId = dto.TipoMascotaId;

            await _context.SaveChangesAsync();

            habitacion = await _context.Habitaciones
                .Include(h => h.TipoMascota)
                .FirstAsync(h => h.Id == id);

            return Ok(new
            {
                success = true,
                data = habitacion,
                message = "Habitación actualizada correctamente."
            });
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Habitación no encontrada."
                });
            }

            _context.Habitaciones.Remove(habitacion);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Habitación eliminada correctamente."
            });
        }
    }
}