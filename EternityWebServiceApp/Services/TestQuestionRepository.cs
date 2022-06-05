using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class TestQuestionRepository : IImageRepository<TestQuestion>
    {
        private readonly EternityDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public TestQuestionRepository(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IEnumerable<TestQuestion> Get()
        {
            return _context.TestQuestions.ToList();
        }

        public TestQuestion Get(int id)
        {
            return _context.TestQuestions.FirstOrDefault(x => x.TestQuestionId == id);
        }

        public void Create(TestQuestion testQuestion, IFormFileCollection uploadedFiles)
        {
            _context.TestQuestions.Add(testQuestion);
            _context.SaveChanges();
            if (uploadedFiles.Count > 0)
            {
                string path = $"{_appEnvironment.WebRootPath}/Images/TestQuestions/{testQuestion.TestQuestionId}";
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

        public void Update(TestQuestion testQuestion, IFormFileCollection uploadedFiles)
        {
            _context.TestQuestions.Update(testQuestion);
            _context.SaveChanges();
            if (uploadedFiles.Count > 0)
            {
                string path = $"{_appEnvironment.WebRootPath}/Images/TestQuestions/{testQuestion.TestQuestionId}";
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
            _context.TestQuestions.Remove(Get(id));
            _context.SaveChanges();
            string path = $"{_appEnvironment.WebRootPath}/Images/TestQuestions/{id}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
