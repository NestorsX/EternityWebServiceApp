using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class AttractionController : Controller
    {
        private readonly IRepository<Attraction> _repository;

        public AttractionController(IRepository<Attraction> repository)
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
        public IActionResult CreateAsync(Attraction newCity)
        {
            if (ModelState.IsValid)
            {
                var attraction = new Attraction
                {
                    AttractionId = null,
                    Title = newCity.Title,
                    Description = newCity.Description
                };

                _repository.Create(attraction);
                return RedirectToAction("Index");
            }

            return View(newCity);
        }

        public ActionResult Edit(int id)
        {
            Attraction attraction = _repository.Get(id);
            if (attraction != null)
                return View(attraction);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Attraction newCity)
        {
            Attraction attraction = _repository.Get((int)newCity.AttractionId);
            if (ModelState.IsValid)
            {
                attraction.Title = newCity.Title;
                attraction.Description = newCity.Description;
                _repository.Update(attraction);
                return RedirectToAction("Index");
            }

            return View(newCity);
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
