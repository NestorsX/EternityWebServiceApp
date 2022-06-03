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
    public class CitiesController : ControllerBase
    {
        private readonly EternityDBContext _context;

        public CitiesController(EternityDBContext context)
        {
            _context = context;
        }

        // Получает список городов
        [HttpGet]
        public async Task<IEnumerable<CityViewModel>> Get()
        {
            IEnumerable<CityViewModel> result = new List<CityViewModel>();
            IEnumerable<City> cities = await _context.Cities.ToListAsync();
            foreach (var item in cities)
            {
                result = result.Append(new CityViewModel
                {
                    CityId = item.CityId,
                    Title = item.Title,
                    Description = item.Description,
                    References = await _context.DataReferences.Where(x => x.CityId == item.CityId).Select(x => x.AttractionId).ToListAsync()
                });
            }

            return result;
        }

        // Получает город по id
        [HttpGet("{id}")]
        public async Task<ActionResult<CityViewModel>> Get(int id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(x => x.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            var result = new CityViewModel
            {
                CityId = city.CityId,
                Title = city.Title,
                Description = city.Description,
                References = await _context.DataReferences.Where(x => x.CityId == city.CityId).Select(x => x.AttractionId).ToListAsync()
            };

            return Ok(result);
        }
    }
}
