using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class DataReference
    {
        [Key]
        public int? DataReferenceId { get; set; }
        public int CityId { get; set; }
        public int AttractionId { get; set; }
    }
}
