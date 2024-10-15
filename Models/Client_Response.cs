using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SimposioBack.Models.Dtos;

namespace SimposioBack.Models
{
    public class Client_Response
    {
        public int Id_Cliente { get; set; }

        public string Nombre_Cliente { get; set; }

        public int Numero_Invitados { get; set; }

        public string Estado { get; set; }

        public int Id_Evento { get; set; }

        public List<InvitadoRegistroDto> Invitados { get; set; }

    }
}
