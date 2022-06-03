using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class ActionCategory
    {
        [Key]
        public int? ActionCategoryId { get; set; }
        public string Action { get; set; }
    }
}
