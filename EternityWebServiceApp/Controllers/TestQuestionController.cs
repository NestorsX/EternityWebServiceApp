using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class TestQuestionController : Controller
    {
        private readonly IImageRepository<TestQuestion> _repository;

        public TestQuestionController(IImageRepository<TestQuestion> repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.Get());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TestQuestion newTestQuestion, IFormFileCollection uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                var testQuestion = new TestQuestion
                {
                    TestQuestionId = null,
                    Question = newTestQuestion.Question,
                    RightAnswer = newTestQuestion.RightAnswer,
                    WrongAnswer1 = newTestQuestion.WrongAnswer1,
                    WrongAnswer2 = newTestQuestion.WrongAnswer2,
                    WrongAnswer3 = newTestQuestion.WrongAnswer3,
                };

                _repository.Create(testQuestion, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newTestQuestion);
        }

        public ActionResult Edit(int id)
        {
            TestQuestion testQuestion = _repository.Get(id);
            if (testQuestion != null)
                return View(testQuestion);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(TestQuestion newTestQuestion, IFormFileCollection uploadedFiles)
        {
            TestQuestion testQuestion = _repository.Get((int)newTestQuestion.TestQuestionId);
            if (ModelState.IsValid)
            {
                testQuestion.Question = newTestQuestion.Question;
                testQuestion.RightAnswer = newTestQuestion.RightAnswer;
                testQuestion.WrongAnswer1 = newTestQuestion.WrongAnswer1;
                testQuestion.WrongAnswer2 = newTestQuestion.WrongAnswer2;
                testQuestion.WrongAnswer3 = newTestQuestion.WrongAnswer3;
                _repository.Update(testQuestion, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newTestQuestion);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            TestQuestion testQuestion = _repository.Get(id);
            if (testQuestion != null)
            {
                return View(testQuestion);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
