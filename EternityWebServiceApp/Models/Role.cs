using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class Role
    {
        [Key]
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название роли")]
        public string Name { get; set; }
    }
}
