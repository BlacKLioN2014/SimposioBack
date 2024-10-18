using System.ComponentModel.DataAnnotations;

namespace SimposioBack.Models
{
    public class _ids_Invitados_
    {
        [Required(ErrorMessage ="Es nesesaria la lista de ids")] 
        public List<int> Ids { get; set; }
    }
}
