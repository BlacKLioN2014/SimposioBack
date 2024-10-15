using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models
{
    public class Evento
    {
        [Key]
        public int Id_Evento { get; set; }

        [Required]
        public string Nombre_Evento { get; set; }

        [Required]
        public bool Estatus { get; set; }
    }
}
