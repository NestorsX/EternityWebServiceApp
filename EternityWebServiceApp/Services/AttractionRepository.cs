using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class AttractionRepository : IImageRepository<Attraction>
    {
        private readonly EternityDBContext _context;
        IWebHostEnvironment _appEnvironment;

        public AttractionRepository(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IEnumerable<Attraction> Get()
        {
            return _context.Attractions.ToList().OrderBy(x => x.AttractionId);
        }

        public Attraction Get(int id)
        {
            return _context.Attractions.FirstOrDefault(x => x.AttractionId == id);
        }

        public void Create(Attraction attraction, IFormFileCollection uploadedFiles)
        {
            _context.Attractions.Add(attraction);
            _context.SaveChanges();
            if (uploadedFiles.Count > 0)
            {
                string path = $"{_appEnvironment.WebRootPath}/Images/Attractions/{attraction.AttractionId}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var file in uploadedFiles)
                {
                    string filePath = $"{path}/{file.FileName}";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }
        }

        public void Update(Attraction attraction, IFormFileCollection uploadedFiles)
        {
            _context.Attractions.Update(attraction);
            _context.SaveChanges();
            if (uploadedFiles.Count > 0)
            {
                string path = $"{_appEnvironment.WebRootPath}/Images/Attractions/{attraction.AttractionId}";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Directory.CreateDirectory(path);
                foreach (var file in uploadedFiles)
                {
                    string filePath = $"{path}/{file.FileName}";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }
        }

        public void Delete(int id)
        {
            _context.Attractions.Remove(Get(id));
            _context.SaveChanges();
            string path = $"{_appEnvironment.WebRootPath}/Images/Attractions/{id}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
