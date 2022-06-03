using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class DataAction
    {
        [Key]
        public int? DataActionId { get; set; }
        public int UserId { get; set; }
        public int DataCategoryId { get; set; }
        public int ActionCategoryId { get; set; }
        public int? ItemId { get; set; }
    }
}
