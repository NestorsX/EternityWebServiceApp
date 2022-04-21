using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class CityRepository : IImageRepository<City>
    {
        private readonly EternityDBContext _context;
        IWebHostEnvironment _appEnvironment;

        public CityRepository(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IEnumerable<City> Get()
        {
            return _context.Cities.ToList().OrderBy(x => x.CityId);
        }

        public City Get(int id)
        {
            return _context.Cities.FirstOrDefault(x => x.CityId == id);
        }

        public void Create(City city, IFormFileCollection uploadedFiles)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            if (uploadedFiles.Count > 0)
            {
                if (!Directory.Exists($"{_appEnvironment.WebRootPath}\\Images\\Cities\\{city.CityId}"))
                {
                    Directory.CreateDirectory($"{_appEnvironment.WebRootPath}\\Images\\Cities\\{city.CityId}");
                }

                var imageCount = 0;
                foreach (var file in uploadedFiles)
                {
                    imageCount++;
                    string path = $"{_appEnvironment.WebRootPath}/Images/Cities/{city.CityId}/{imageCount}.jpg";
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                city.ImageFolderPath = $"Images/Cities/{city.CityId}";
                city.ImageCount = imageCount;
                _context.Cities.Update(city);
                _context.SaveChanges();
            }
        }

        public void Update(City city)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Cities.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
