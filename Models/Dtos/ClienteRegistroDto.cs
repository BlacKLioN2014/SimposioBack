using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models.Dtos
{
    public class ClienteRegistroDto
    {

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El numero de invitados es obligatorio")]
        public int Numero_Invitados { get; set; }

        [Required(ErrorMessage = "El estado es obligatoria")]
        public string Estado { get; set; }

    }
}
