using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class City
    {
        [Key]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите описание")]
        public string Description { get; set; }
    }
}
