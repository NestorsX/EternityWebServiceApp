using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly EternityDBContext _context;
        IWebHostEnvironment _appEnvironment;

        public ImagesController(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // Получение списка картинок в конкретной папке (например /Cities/1)
        [HttpGet("{category}/{id}")]
        public ActionResult<IEnumerable<string>> Get(string category, string id)
        {
            string path = $"{_appEnvironment.WebRootPath}/Images/{category}/{id}";
            if (!Directory.Exists(path))
            {
                return NotFound();
            }

            return Directory.GetFiles(path).Select(x => Path.GetFileName(x)).ToList();
        }

        // Добавление аватара пользователю
        [HttpPost]
        public ActionResult Post(int userId, IFormFile image)
        {
            if (_context.Users.FirstOrDefault(x => x.UserId == userId) == null)
            {
                return BadRequest();
            }

            if (image == null)
            {
                return BadRequest();
            }

            string path = $"{_appEnvironment.WebRootPath}/Images/Users/{userId}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
            string filePath = $"{path}/{image.FileName}";
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return Ok();
        }
    }
}
