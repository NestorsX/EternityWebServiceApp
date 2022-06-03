using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.ViewModels;
using System.Linq;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        private readonly EternityDBContext _context;

        public AttractionsController(EternityDBContext context)
        {
            _context = context;
        }

        // Получает список достопримечательностей
        [HttpGet]
        public async Task<IEnumerable<AttractionViewModel>> Get()
        {
            IEnumerable<AttractionViewModel> result = new List<AttractionViewModel>();
            IEnumerable<Attraction> attractions = await _context.Attractions.ToListAsync();
            foreach (var item in attractions)
            {
                var reference = await _context.DataReferences.FirstOrDefaultAsync(x => x.AttractionId == item.AttractionId);
                result = result.Append(new AttractionViewModel
                {
                    AttractionId = item.AttractionId,
                    Title = item.Title,
                    Description = item.Description,
                    Reference = reference?.CityId
                });
            }

            return result;
        }

        // Получает достопримечательность по id
        [HttpGet("{id}")]
        public async Task<ActionResult<AttractionViewModel>> Get(int id)
        {
            Attraction attraction = await _context.Attractions.FirstOrDefaultAsync(x => x.AttractionId == id);
            if (attraction == null)
            {
                return NotFound();
            }

            var reference = await _context.DataReferences.FirstOrDefaultAsync(x => x.AttractionId == attraction.AttractionId);
            var result = new AttractionViewModel
            {
                AttractionId = attraction.AttractionId,
                Title = attraction.Title,
                Description = attraction.Description,
                Reference = reference?.CityId
            };

            return Ok(result);
        }
    }
}
