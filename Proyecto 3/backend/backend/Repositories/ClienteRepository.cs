using backend.Models;
using backend.Repositories;

namespace backend.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        // Lista estática simulada para trabajar en tu rama sin BD
        private static List<Cliente> _clientesSimulados = new List<Cliente>
        {
            new Cliente { Id = 1, Nombre = "Doris Navas", Correo = "doris@salem.com", Role = "ADMIN", Activo = true },
            new Cliente { Id = 2, Nombre = "Pedro Arce", Correo = "pedro@salem.com", Role = "EMPLOYEE", Activo = true }
        };

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await Task.FromResult(_clientesSimulados.Where(c => c.Activo));
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            var cliente = _clientesSimulados.FirstOrDefault(c => c.Id == id && c.Activo);
            return await Task.FromResult(cliente);
        }

        public async Task AddAsync(Cliente cliente)
        {
            cliente.Id = _clientesSimulados.Count + 1;
            cliente.Activo = true;
            _clientesSimulados.Add(cliente);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            var index = _clientesSimulados.FindIndex(c => c.Id == cliente.Id);
            if (index != -1)
            {
                _clientesSimulados[index] = cliente;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteLogicalAsync(int id)
        {
            var cliente = _clientesSimulados.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                cliente.Activo = false; // Borrado lógico
            }
            await Task.CompletedTask;
        }
    }
}