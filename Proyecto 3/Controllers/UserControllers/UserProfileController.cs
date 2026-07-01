using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_3.Data;
using Proyecto_3.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace Proyecto_3.Controllers
{
    [Route("api/user/profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<object> _passwordHasher = new();

        public UserProfileController(AppDbContext context)
        {
            _context = context;
        }

        // 1. OBTENER EL PERFIL DEL CLIENTE
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var usuario = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Role = u.Role
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound(new { success = false, message = "Perfil no encontrado" });

            return Ok(new { success = true, data = usuario });
        }

        // 2. ACTUALIZAR EL PERFIL (CON CONTRASEÑA OPCIONAL)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, UserProfileUpdateDto dto)
        {
            var usuario = await _context.Users.FindAsync(id);

            if (usuario == null)
                return NotFound(new { success = false, message = "Usuario no encontrado" });

            // Validar que el correo no lo esté usando otra persona
            var correoExiste = await _context.Users
                .AnyAsync(u => u.Correo == dto.Correo && u.Id != id);

            if (correoExiste)
                return BadRequest(new { success = false, message = "El correo ya está registrado por otro usuario" });

            // Actualizar datos básicos
            usuario.Nombre = dto.Nombre;
            usuario.Correo = dto.Correo;

            // REQUISITO DE LA RÚBRICA: Si la contraseña viene vacía, no se toca
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuario.Password = _passwordHasher.HashPassword(usuario, dto.Password);
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Perfil actualizado correctamente" });
        }

        [HttpGet("{id}/mascotas")]
public async Task<IActionResult> GetMascotasUsuario(int id)
{
    // Buscamos las mascotas que le pertenecen al ID del usuario conectado
    // Nota: Asegúrate de que el campo en tu modelo Mascota se llame UsuarioId o IdUsuario
    var mascotas = await _context.Mascotas
        .Where(m => m.Id == id)
        .ToListAsync();

    return Ok(new { success = true, data = mascotas });
}

[HttpGet("{id}/citas")]
public async Task<IActionResult> GetCitasUsuario(int id)
{
    // Buscamos las citas que le pertenecen al ID del usuario conectado
    var citas = await _context.Citas
        .Where(c => c.Id == id)
        .ToListAsync();

    return Ok(new { success = true, data = citas });
}
    }
    
}