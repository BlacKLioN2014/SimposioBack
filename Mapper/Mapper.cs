using AutoMapper;
using SimposioBack.Models;
using SimposioBack.Models.Dtos;

namespace SimposioBack.Mapper
{
    public class Mapper :Profile
    {

        public Mapper()
        {
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();
            CreateMap<Usuario, UsuarioRegistroDto>().ReverseMap();
            CreateMap<InvitadosExtra, InvitadoExtraRegistroDto>().ReverseMap();
        }

    }
}
