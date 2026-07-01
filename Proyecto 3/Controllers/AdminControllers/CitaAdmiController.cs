using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_3.Data;
using Proyecto_3.DTOs.Citas;
using Proyecto_3.Models;

namespace Proyecto_3.Controllers.AdminControllers
{
    [Route("api/admin/citas")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class CitaAdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitaAdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCitas()
        {
            var citas = await _context.Citas
                .Include(c => c.User)
                .Include(c => c.Mascota)
                .Include(c => c.Habitacion)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data = citas,
                message = "Citas obtenidas correctamente."
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCita(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.User)
                .Include(c => c.Mascota)
                .Include(c => c.Habitacion)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Cita no encontrada."
                });
            }

            return Ok(new
            {
                success = true,
                data = cita,
                message = "Cita encontrada."
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCita(CitaCreateDto dto)
        {
            if (dto.FechaSalida < dto.FechaInicio)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La fecha de salida no puede ser menor que la fecha de inicio."
                });
            }

            var userExiste = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El usuario no existe."
                });
            }

            var mascotaExiste = await _context.Mascotas.AnyAsync(m => m.Id == dto.MascotaId);
            if (!mascotaExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La mascota no existe."
                });
            }

            var habitacionExiste = await _context.Habitaciones.AnyAsync(h => h.Id == dto.HabitacionId);
            if (!habitacionExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La habitación no existe."
                });
            }

            var mascotaYaTieneCita = await _context.Citas
                .AnyAsync(c =>
                    c.MascotaId == dto.MascotaId &&
                    c.FechaSalida.Date >= DateTime.Today);

            if (mascotaYaTieneCita)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La mascota ya tiene una cita activa."
                });
            }

            var habitacionOcupada = await HabitacionOcupada(
                dto.HabitacionId,
                dto.FechaInicio,
                dto.FechaSalida
            );

            if (habitacionOcupada)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La habitación ya está ocupada en ese rango de fechas."
                });
            }

            var cita = new Cita
            {
                FechaInicio = dto.FechaInicio,
                FechaSalida = dto.FechaSalida,
                UserId = dto.UserId,
                MascotaId = dto.MascotaId,
                HabitacionId = dto.HabitacionId
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            cita = await _context.Citas
                .Include(c => c.User)
                .Include(c => c.Mascota)
                .Include(c => c.Habitacion)
                .FirstAsync(c => c.Id == cita.Id);

            return CreatedAtAction(nameof(GetCita), new { id = cita.Id }, new
            {
                success = true,
                data = cita,
                message = "Cita creada correctamente."
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCita(int id, CitaUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El ID no coincide."
                });
            }

            var cita = await _context.Citas.FindAsync(id);

            if (cita == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Cita no encontrada."
                });
            }

            if (dto.FechaSalida < dto.FechaInicio)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La fecha de salida no puede ser menor que la fecha de inicio."
                });
            }

            var userExiste = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El usuario no existe."
                });
            }

            var mascotaExiste = await _context.Mascotas.AnyAsync(m => m.Id == dto.MascotaId);
            if (!mascotaExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La mascota no existe."
                });
            }

            var habitacionExiste = await _context.Habitaciones.AnyAsync(h => h.Id == dto.HabitacionId);
            if (!habitacionExiste)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La habitación no existe."
                });
            }

            var habitacionOcupada = await HabitacionOcupada(
                dto.HabitacionId,
                dto.FechaInicio,
                dto.FechaSalida,
                id
            );

            if (habitacionOcupada)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La habitación ya está ocupada en ese rango de fechas."
                });
            }

            cita.FechaInicio = dto.FechaInicio;
            cita.FechaSalida = dto.FechaSalida;
            cita.UserId = dto.UserId;
            cita.MascotaId = dto.MascotaId;
            cita.HabitacionId = dto.HabitacionId;

            await _context.SaveChangesAsync();

            cita = await _context.Citas
                .Include(c => c.User)
                .Include(c => c.Mascota)
                .Include(c => c.Habitacion)
                .FirstAsync(c => c.Id == id);

            return Ok(new
            {
                success = true,
                data = cita,
                message = "Cita actualizada correctamente."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);

            if (cita == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Cita no encontrada."
                });
            }

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Cita eliminada correctamente."
            });
        }

        private async Task<bool> HabitacionOcupada(
            int habitacionId,
            DateTime fechaInicio,
            DateTime fechaSalida,
            int? citaIdIgnorar = null)
        {
            return await _context.Citas
                .Where(c => c.HabitacionId == habitacionId)
                .Where(c => citaIdIgnorar == null || c.Id != citaIdIgnorar)
                .AnyAsync(c =>
                    c.FechaInicio <= fechaSalida &&
                    c.FechaSalida >= fechaInicio);
        }
    }
}