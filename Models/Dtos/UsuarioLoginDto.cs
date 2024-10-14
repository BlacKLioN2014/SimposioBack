using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; }
    }
}
