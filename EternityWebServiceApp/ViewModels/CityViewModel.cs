using System.Collections.Generic;

namespace EternityWebServiceApp.ViewModels
{
    public class CityViewModel
    {
        public int? CityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<int> References { get; set; }
    }
}
