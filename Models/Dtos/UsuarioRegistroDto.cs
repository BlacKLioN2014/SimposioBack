using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models.Dtos
{
    public class UsuarioRegistroDto
    {

        [Required(ErrorMessage = "El correo es obligatorio")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; }

    }
}
