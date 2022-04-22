using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly EternityDBContext _context;

        public CitiesController(EternityDBContext context)
        {
            _context = context;
        }

        // Получает список городов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> Get()
        {
            return await _context.Cities.ToListAsync();
        }

        // Получает город по id
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> Get(int id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(x => x.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return new ObjectResult(city);
        }
    }
}
