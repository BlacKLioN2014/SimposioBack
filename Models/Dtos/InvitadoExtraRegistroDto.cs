using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models.Dtos
{
    public class InvitadoExtraRegistroDto
    {

        [Required]
        public int Id_Cliente { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido_Paterno { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido_Materno { get; set; }

        public string Cliente_Externo { get; set; }
    }
}
