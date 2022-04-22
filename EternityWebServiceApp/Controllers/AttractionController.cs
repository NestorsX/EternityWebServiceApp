using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class AttractionController : Controller
    {
        private readonly IImageRepository<Attraction> _repository;

        public AttractionController(IImageRepository<Attraction> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Get());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAsync(Attraction newAttraction, IFormFileCollection uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                var attraction = new Attraction
                {
                    AttractionId = null,
                    Title = newAttraction.Title,
                    Description = newAttraction.Description
                };

                _repository.Create(attraction, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newAttraction);
        }

        public ActionResult Edit(int id)
        {
            Attraction attraction = _repository.Get(id);
            if (attraction != null)
                return View(attraction);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Attraction newAttraction, IFormFileCollection uploadedFiles)
        {
            Attraction attraction = _repository.Get((int)newAttraction.AttractionId);
            if (ModelState.IsValid)
            {
                attraction.Title = newAttraction.Title;
                attraction.Description = newAttraction.Description;
                _repository.Update(attraction, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newAttraction);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Attraction attraction = _repository.Get(id);
            if (attraction != null)
            {
                return View(attraction);
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
