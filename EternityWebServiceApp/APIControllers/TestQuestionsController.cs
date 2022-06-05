using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.ViewModels;
using System.Linq;
using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQuestionsController : ControllerBase
    {
        private readonly EternityDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public TestQuestionsController(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // Получает список из 10 вопросов для теста
        [HttpGet]
        public async Task<IEnumerable<TestQuestionViewModel>> Get()
        {
            var result = new List<TestQuestionViewModel>();
            List<TestQuestion> questions = await _context.TestQuestions.ToListAsync();
            var random = new Random();
            var shuffled = questions.OrderBy(x => random.Next()).ToList();
            shuffled = shuffled.Take(10).ToList();

            foreach (var item in shuffled)
            {
                string image = null;
                var path = $"{_appEnvironment.WebRootPath}/Images/TestQuestions/{item.TestQuestionId}";
                if (Directory.Exists(path))
                {
                    image = $"{Directory.GetFiles(path).Select(x => Path.GetFileName(x)).First()}";
                }

                result.Add(new TestQuestionViewModel
                {
                    TestQuestionId = item.TestQuestionId,
                    Question = item.Question,
                    RightAnswer = item.RightAnswer,
                    WrongAnswer1 = item.WrongAnswer1,
                    WrongAnswer2 = item.WrongAnswer2,
                    WrongAnswer3 = item.WrongAnswer3,
                    Image = image,
                });
            }

            return result;
        }
    }
}
