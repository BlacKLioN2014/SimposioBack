using AutoMapper;
using SimposioBack.Data;
using SimposioBack.Models;
using SimposioBack.Models.Dtos;
using SimposioBack.Repository.IRepository;

namespace SimposioBack.Repository
{
    public class ClienteRepository : IClienteRepository
    {

        private readonly ApplicationDbContext _bd;

        private readonly IMapper _Mapper;


        public ClienteRepository(ApplicationDbContext db, IMapper Mapper)
        {
            _bd = db;
            _Mapper = Mapper;
        }


        public Cliente GetCliente(int ClienteId)
        {
            return _bd.Cliente.FirstOrDefault(c => c.Id_Cliente == ClienteId);
        }


        public ICollection<Cliente> GetClientes()
        {
            return _bd.Cliente.OrderBy(c => c.Id_Cliente).ToList();
        }


        public bool IsUniqueCliente(int ClienteId)
        {
            var UsuarioBd = _bd.Cliente.FirstOrDefault(u => u.Id_Cliente == ClienteId);

            if (UsuarioBd == null) 
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public bool NombreExist(string Nombre)
        {
            var UsuarioBd = _bd.Usuario.FirstOrDefault(u => u.Nombre.ToLower() == Nombre.ToLower());

            if (UsuarioBd == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public async /*Task<Usuario>*/ Task<bool> Registro(ClienteRegistroDto clienteRegistroDto)
        {
            string result = string.Empty;

            Cliente cliente = new Cliente()
            {
                Nombre_Cliente = clienteRegistroDto.Nombre,
                Numero_Invitados = clienteRegistroDto.Numero_Invitados,
                Estado = clienteRegistroDto.Estado,
                Id_Evento = clienteRegistroDto.Id_Evento,
                Mesa = clienteRegistroDto.Mesa,
            };

            var AgregarCliente = _bd.Add(cliente);

            var a  = _bd.SaveChanges();

            result = AgregarCliente.State.ToString();

            if(result == "Unchanged")
            {
                return true;
            }

            return false;
        }


    }
}
