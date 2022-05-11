using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;
using System.Linq;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarksController : ControllerBase
    {
        private readonly EternityDBContext _context;

        public BookmarksController(EternityDBContext context)
        {
            _context = context;
        }

        // Получает список городов
        [HttpGet("citybookmark/{id}")]
        public async Task<ActionResult<IEnumerable<CityBookmark>>> GetCityBookmarks(int id)
        {
            return await _context.CityBookmarks.Where(x => x.UserId == id).ToListAsync();
        }

        // Добавление закладки для города
        [HttpPost("citybookmark")]
        public async Task<ActionResult> PostCityBookmark(CityBookmark bookmark)
        {
            if (bookmark == null)
            {
                return BadRequest();
            }

            if (!await _context.Cities.AnyAsync(x => x.CityId == bookmark.CityId))
            {
                return NotFound();
            }

            if (!await _context.Users.AnyAsync(x => x.UserId == bookmark.UserId))
            {
                return NotFound();
            }

            if (await _context.CityBookmarks.FirstOrDefaultAsync(x => x.UserId == bookmark.UserId && x.CityId == bookmark.CityId) != null)
            {
                return Ok();
            }

            bookmark.CityBookmarkId = null;
            _context.CityBookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Удаление закладки города
        [HttpDelete("citybookmark/{id}")]
        public async Task<ActionResult> DeleteCityBookmark(int id)
        {
            CityBookmark bookmark = await _context.CityBookmarks.FirstOrDefaultAsync(x => x.CityBookmarkId == id);
            if (bookmark == null)
            {
                return NotFound();
            }

            _context.CityBookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Получает список достопримечательностей
        [HttpGet("attractionbookmark/{id}")]
        public async Task<ActionResult<IEnumerable<AttractionBookmark>>> GetAttractionBookmarks(int id)
        {
            return await _context.AttractionBookmarks.Where(x => x.UserId == id).ToListAsync();
        }

        // Добавление закладки для достопримечательности
        [HttpPost("attractionbookmark")]
        public async Task<ActionResult> PostAttractionBookmark(AttractionBookmark bookmark)
        {
            if (bookmark == null)
            {
                return BadRequest();
            }

            if (!await _context.Attractions.AnyAsync(x => x.AttractionId == bookmark.AttractionId))
            {
                return NotFound();
            }

            if (!await _context.Users.AnyAsync(x => x.UserId == bookmark.UserId))
            {
                return NotFound();
            }

            if(await _context.AttractionBookmarks.FirstOrDefaultAsync(x => x.UserId == bookmark.UserId && x.AttractionId == bookmark.AttractionId) != null)
            {
                return Ok();
            }

            bookmark.AttractionBookmarkId = null;
            _context.AttractionBookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Удаление закладки города
        [HttpDelete("attractionbookmark/{id}")]
        public async Task<ActionResult> DeleteAttractionBookmark(int id)
        {
            AttractionBookmark bookmark = await _context.AttractionBookmarks.FirstOrDefaultAsync(x => x.AttractionBookmarkId == id);
            if (bookmark == null)
            {
                return NotFound();
            }

            _context.AttractionBookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
