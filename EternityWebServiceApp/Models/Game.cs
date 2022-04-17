using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class Game
    {
        [Key]
        public int? GameId { get; set; }

        [Required(ErrorMessage = "Введите название игры")]
        public string Name { get; set; }
    }
}
