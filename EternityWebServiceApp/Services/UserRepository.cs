using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly EternityDBContext _context;
        IWebHostEnvironment _appEnvironment;

        public UserRepository(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IEnumerable<User> Get()
        {
            return _context.Users.ToList().OrderBy(x => x.UserId);
        }

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(x => x.UserId == id);
        }

        public void Create(User user, IFormFile uploadedFile)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            if (uploadedFile != null)
            {
                string path = $"{_appEnvironment.WebRootPath}/Images/Users/{user.UserId}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = $"{path}/{uploadedFile.FileName}";
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }
            }
        }

        public void Update(User user, IFormFile uploadedFile)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            if (uploadedFile != null)
            {
                string path = $"{_appEnvironment.WebRootPath}/Images/Users/{user.UserId}";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Directory.CreateDirectory(path);
                string filePath = $"{path}/{uploadedFile.FileName}";
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }
            }
        }


        public void Delete(int id)
        {
            _context.Users.Remove(Get(id));
            _context.SaveChanges();
            string path = $"{_appEnvironment.WebRootPath}/Images/Users/{id}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
