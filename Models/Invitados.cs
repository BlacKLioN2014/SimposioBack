using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimposioBack.Models
{
    public class Invitados
    {
        [Key]
        public int Id_Invitado { get; set; }

        [Required]
        public int Id_Cliente { get; set; }

        [ForeignKey("Id_Cliente")]
        public Cliente Cliente { get; set; }

        [Required]
        public string Nombre_Invitado { get; set; }

        [Required]
        public bool Asistencia { get; set; }
    }
}
