using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;

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
        public async Task<ActionResult<IEnumerable<Attraction>>> Get()
        {
            return await _context.Attractions.ToListAsync();
        }

        // Получает достопримечательность по id
        [HttpGet("{id}")]
        public async Task<ActionResult<Attraction>> Get(int id)
        {
            Attraction attraction = await _context.Attractions.FirstOrDefaultAsync(x => x.AttractionId == id);
            if (attraction == null)
            {
                return NotFound();
            }

            return new ObjectResult(attraction);
        }
    }
}
