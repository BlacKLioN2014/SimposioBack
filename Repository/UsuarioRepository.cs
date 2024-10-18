using AutoMapper;
using SimposioBack.Data;
using SimposioBack.Models;
using SimposioBack.Models.Dtos;
using SimposioBack.Repository.IRepository;

namespace SimposioBack.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly ApplicationDbContext _bd;

        private readonly IMapper _Mapper;


        public UsuarioRepository(ApplicationDbContext db, IMapper Mapper)
        {
            _bd = db;
            _Mapper = Mapper;
        }


        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.Usuario.FirstOrDefault(c => c.Id_Usuario == usuarioId);
        }


        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.Usuario.OrderBy(c => c.Nombre).ToList();
        }


        public bool IsUniqueUsuario(int usuarioId)
        {
            var UsuarioBd = _bd.Usuario.FirstOrDefault(u => u.Id_Usuario == usuarioId);

            if (UsuarioBd == null) 
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public bool EmailExist(string Email)
        {
            var UsuarioBd = _bd.Usuario.FirstOrDefault(u => u.Correo.ToLower() == Email.ToLower());

            if (UsuarioBd == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public  UsuarioLoginRespuestaDto Login(UsuarioLoginDto UsuarioLoginDto)
        {
            var Usuario =  _bd.Usuario.FirstOrDefault( u => u.Correo.ToLower() == UsuarioLoginDto.Correo.ToLower() && u.Contraseña == UsuarioLoginDto.Contraseña);

            if (Usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Usuario = null,
                    Token = ""
                };
            }

            var token = SimposioBack.Data.Methods.getToken(Usuario.Correo, Usuario.Contraseña);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = token,
                Usuario = _Mapper.Map<Usuario>(Usuario),
            };

            return usuarioLoginRespuestaDto;
        }


        public async /*Task<Usuario>*/ Task<bool> Registro(UsuarioRegistroDto UsuarioRegistroDto)
        {
            string result = string.Empty;

            Usuario usuario = new Usuario()
            {
                Correo = UsuarioRegistroDto.Correo,
                Nombre = UsuarioRegistroDto.Nombre,
                Contraseña = UsuarioRegistroDto.Contraseña,
                
            };

            var AgregarUsuario = _bd.Add(usuario);

            var a  = _bd.SaveChanges();

            result = AgregarUsuario.State.ToString();

            if(result == "Unchanged")
            {
                return true;
            }

            return false;
        }


    }
}
