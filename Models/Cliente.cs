using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimposioBack.Models
{
    public class Cliente
    {
        [Key]
        public int Id_Cliente { get; set; }

        [Required]
        public string Nombre_Cliente { get; set; }

        [Required]
        public int Numero_Invitados { get; set; }

        [Required]
        [StringLength(60)]
        public string Estado { get; set; }

        [Required]
        public int Id_Evento { get; set; }

        [ForeignKey("Id_Evento ")]
        public Evento Evento { get; set; }
    }
}
