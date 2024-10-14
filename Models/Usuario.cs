using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }

        [Required]
        [StringLength(60)]
        public string Correo { get; set; }

        [Required]
        [StringLength(60)]
        public string Nombre { get; set; }

        [Required]
        public string Contraseña { get; set; }
    }
}
