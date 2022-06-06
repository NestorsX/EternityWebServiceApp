using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class CityController : Controller
    {
        private readonly IImageRepository<City> _repository;

        public CityController(IImageRepository<City> repository)
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
        public IActionResult Create(City newCity, IFormFileCollection uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                var city = new City
                {
                    CityId = null,
                    Title = newCity.Title,
                    Description = newCity.Description
                };

                _repository.Create(city, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newCity);
        }

        public ActionResult Edit(int id)
        {
            City city = _repository.Get(id);
            if (city != null)
                return View(city);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(City newCity, IFormFileCollection uploadedFiles)
        {
            City city = _repository.Get((int)newCity.CityId);
            if (ModelState.IsValid)
            {
                city.Title = newCity.Title;
                city.Description = newCity.Description;
                _repository.Update(city, uploadedFiles);
                return RedirectToAction("Index");
            }

            return View(newCity);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            City city = _repository.Get(id);
            if (city != null)
            {
                return View(city);
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
