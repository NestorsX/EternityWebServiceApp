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
                string path = $"{_appEnvironment.WebRootPath}/Images/Cities/{city.CityId}";
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

        public void Update(City city, IFormFileCollection uploadedFiles)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();
            if (uploadedFiles.Count > 0)
            {
                string path = $"{_appEnvironment.WebRootPath}/Images/Cities/{city.CityId}";
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
            _context.Cities.Remove(Get(id));
            _context.SaveChanges();
            string path = $"{_appEnvironment.WebRootPath}/Images/Cities/{id}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
