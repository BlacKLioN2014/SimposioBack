using SimposioBack.Models;
using SimposioBack.Models.Dtos;

namespace SimposioBack.Repository.IRepository
{
    public interface IUsuarioRepository
    {

        ICollection<Usuario> GetUsuarios();

        Usuario GetUsuario(int usuarioId);

        bool IsUniqueUsuario(int usuarioId);

        bool EmailExist(string Email);

        UsuarioLoginRespuestaDto Login(UsuarioLoginDto UsuarioLoginDto);

        Task<bool> Registro(UsuarioRegistroDto UsuarioRegistroDto);

    }
}
