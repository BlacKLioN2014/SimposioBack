using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models.Dtos
{
    public class InvitadoRegistroDto
    {
        public int Id_Invitado { get; set; }

        public int Id_Cliente { get; set; }

        public string Cliente { get; set; }

        public string Nombre_Invitado { get; set; }

        public int Mesa { get; set; }

        public bool Asistencia { get; set; }
    }
}
