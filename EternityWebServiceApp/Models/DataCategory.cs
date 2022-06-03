using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class DataCategory
    {
        [Key]
        public int? DataCategoryId { get; set; }
        public string Category { get; set; }
    }
}
