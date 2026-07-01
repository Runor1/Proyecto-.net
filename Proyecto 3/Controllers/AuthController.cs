using backend.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_3.Data;
using Proyecto_3.Services;
using System.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, TokenService tokenService,
       IConfiguration config)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var user = _context.Users
            .FirstOrDefault(u => u.Correo == login.Correo && u.Password ==
           login.Password);

            if (user == null)
                return Unauthorized("Credenciales inválidas");

            var token = _tokenService.GenerateToken(user, _config);
            return Ok(new { token,
                            role = user.Role});
        }


        [Authorize]
        [HttpGet("datos-seguros")]
        public IActionResult GetDatos()
        {
            return Ok("Este endpoint está protegido con JWT");
        }

        [AllowAnonymous]
        [HttpGet("publico")]
        public IActionResult GetPublico()
        {
            return Ok("Este endpoint es público y no requiere token");
        }
    }
}