using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_3.Data;
using Proyecto_3.DTOs.Users;
using Proyecto_3.Models;

namespace Proyecto_3.Controllers.AdminControllers
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class UserAdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UserAdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var usuarios = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Correo,
                    u.Role
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var usuario = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Correo,
                    u.Role
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound(new { success = false, message = "Usuario no encontrado" });

            return Ok(new { success = true, data = usuario, message = "Usuario encontrado" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto dto)
        {
            var correoExiste = await _context.Users.AnyAsync(u => u.Correo == dto.Correo);

            if (correoExiste)
                return BadRequest(new { success = false, message = "El correo ya está registrado" });

            var usuario = new User
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Role = dto.Role
            };

            usuario.Password = _passwordHasher.HashPassword(usuario, dto.Password);

            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = usuario.Id }, new
            {
                success = true,
                data = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.Role
                },
                message = "Usuario creado correctamente"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto dto)
        {
            var usuario = await _context.Users.FindAsync(id);

            if (usuario == null)
                return NotFound(new { success = false, message = "Usuario no encontrado" });

            var correoExiste = await _context.Users
                .AnyAsync(u => u.Correo == dto.Correo && u.Id != id);

            if (correoExiste)
                return BadRequest(new { success = false, message = "El correo ya está registrado por otro usuario" });

            usuario.Nombre = dto.Nombre;
            usuario.Correo = dto.Correo;
            usuario.Role = dto.Role;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuario.Password = _passwordHasher.HashPassword(usuario, dto.Password);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.Role
                },
                message = "Usuario actualizado correctamente"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var usuario = await _context.Users.FindAsync(id);

            if (usuario == null)
                return NotFound(new { success = false, message = "Usuario no encontrado" });

            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Usuario eliminado correctamente" });
        }
    }
}