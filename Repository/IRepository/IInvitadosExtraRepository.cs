using SimposioBack.Models;
using SimposioBack.Models.Dtos;

namespace SimposioBack.Repository.IRepository
{
    public interface IInvitadosExtraRepository
    {

        ICollection<InvitadosExtra> GetInvitadosExtra();

        bool NombreExist(string Nombre);

        Task<bool> Registro(InvitadoExtraRegistroDto invitadoExtra);

    }
}
