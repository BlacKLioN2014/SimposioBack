using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models
{
    public class Cliente
    {
        [Key]
        public int Id_Cliente { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int Numero_Invitados { get; set; }

        [Required]
        [StringLength(60)]
        public string Estado { get; set; }
    }
}
