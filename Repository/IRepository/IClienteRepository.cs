using SimposioBack.Models;
using SimposioBack.Models.Dtos;

namespace SimposioBack.Repository.IRepository
{
    public interface IClienteRepository
    {

        ICollection<Cliente> GetClientes();

        Cliente GetCliente(int ClienteId);

        bool IsUniqueCliente(int ClienteId);

        bool NombreExist(string Nombre);

        Task<bool> Registro(ClienteRegistroDto clienteRegistroDto);

    }
}
