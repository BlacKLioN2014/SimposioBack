using AutoMapper;
using SimposioBack.Data;
using SimposioBack.Models;
using SimposioBack.Models.Dtos;
using SimposioBack.Repository.IRepository;

namespace SimposioBack.Repository
{
    public class InvitadosExtraRepository : IInvitadosExtraRepository
    {

        private readonly ApplicationDbContext _bd;

        private readonly IMapper _Mapper;

        public InvitadosExtraRepository(ApplicationDbContext db, IMapper Mapper)
        {
            _bd = db;
            _Mapper = Mapper;
        }

        public ICollection<InvitadosExtra> GetInvitadosExtra()
        {
            return _bd.InvitadosExtras.OrderBy(c => c.Nombre).ToList();
        }

        public bool NombreExist(string Nombre)
        {
            var UsuarioBd = _bd.InvitadosExtras.FirstOrDefault(u => u.Nombre.ToLower() == Nombre.ToLower());

            if (UsuarioBd == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> Registro(InvitadoExtraRegistroDto InvitadoExtraRegistroDto)
        {
            string result = string.Empty;

            InvitadosExtra InvitadoExtra = new InvitadosExtra()
            {
                Id_Cliente = InvitadoExtraRegistroDto.Id_Cliente,
                Nombre = InvitadoExtraRegistroDto.Nombre,
                Apellido_Paterno = InvitadoExtraRegistroDto.Apellido_Paterno,
                Apellido_Materno = InvitadoExtraRegistroDto.Apellido_Materno,
                Cliente_Externo = InvitadoExtraRegistroDto.Cliente_Externo,
            };

            var AgregarInvitadoExtra = _bd.Add(InvitadoExtra);

            var a  = _bd.SaveChanges();

            result = AgregarInvitadoExtra.State.ToString();

            if(result == "Unchanged")
            {
                return true;
            }

            return false;
        }

    }
}
