using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class Attraction
    {
        [Key]
        public int? AttractionId { get; set; }

        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите описание")]
        public string Description { get; set; }

        public string ImageFolderPath { get; set; }

        public int ImageCount { get; set; }
    }
}
