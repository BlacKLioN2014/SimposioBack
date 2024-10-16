﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models
{
    public class InvitadosExtra
    {
        [Key]
        public int Id_Invitado_Extra { get; set; }

        [Required]
        public int Id_Cliente { get; set; }

        [ForeignKey("Id_Cliente")]
        public Cliente Cliente { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido_Paterno { get; set; }

        [StringLength(50)]
        public string Apellido_Materno { get; set; }

        public string Cliente_Externo { get; set; }
    }
}
