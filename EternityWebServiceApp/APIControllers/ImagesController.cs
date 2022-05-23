using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Http;
using EternityWebServiceApp.Models;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly EternityDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ImagesController(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // Получение списка картинок в конкретной папке (например /Cities/1)
        [HttpGet("{category}/{id}")]
        public ActionResult<IEnumerable<Image>> Get(string category, string id)
        {
            string path = $"{_appEnvironment.WebRootPath}/Images/{category}/{id}";
            if (!Directory.Exists(path))
            {
                return NotFound();
            }

            IEnumerable<string> imageNames = Directory.GetFiles(path).Select(x => Path.GetFileName(x)).ToList();
            IEnumerable<Image> images = new List<Image>();
            foreach (string imageName in imageNames)
            {
                images = images.Append(new Image() { Path = imageName });
            }

            return new ObjectResult(images);
        }

        // Добавление аватара пользователю
        [HttpPost("{id}")]
        public ActionResult Post(int id, IFormFile image)
        {
            if (_context.Users.FirstOrDefault(x => x.UserId == id) == null)
            {
                return BadRequest();
            }

            if (image == null)
            {
                return BadRequest();
            }

            string path = $"{_appEnvironment.WebRootPath}/Images/Users/{id}";
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
