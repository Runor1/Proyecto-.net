using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController(IClienteRepository clienteRepository) : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository = clienteRepository;

        // GET: api/clientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return Ok(clientes);
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null) return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(cliente);
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _clienteRepository.AddAsync(cliente);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            // 1. Validar que el ID de la URL coincida con el del cuerpo JSON
            if (id != cliente.Id)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el del cuerpo." });

            // 2. Buscar si el cliente realmente existe y está activo
            var existente = await _clienteRepository.GetByIdAsync(id);
            if (existente == null)
                return NotFound(new { mensaje = "El usuario no existe o fue dado de baja." });

            // 3. Mapear los campos editables del formulario original de la guardería
            existente.Nombre = cliente.Nombre;
            existente.Correo = cliente.Correo;
            existente.Role = cliente.Role;

            // 4. Lógica de contraseña opcional (Vacía mantiene la anterior)
            if (!string.IsNullOrWhiteSpace(cliente.Password))
            {
                existente.Password = cliente.Password;
            }

            // 5. Guardar los cambios actualizados en el repositorio
            await _clienteRepository.UpdateAsync(existente);
            return Ok(new { mensaje = "Usuario actualizado con éxito." });
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await _clienteRepository.GetByIdAsync(id);
            if (existente == null) return NotFound(new { mensaje = "El usuario no existe" });

            await _clienteRepository.DeleteLogicalAsync(id);
            return Ok(new { mensaje = "Usuario dado de baja correctamente" });
        }
    }
}
