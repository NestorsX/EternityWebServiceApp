﻿using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class CityController : Controller
    {
        private readonly IRepository<City> _repository;

        public CityController(IRepository<City> repository)
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
        public IActionResult CreateAsync(City newCity)
        {
            if (ModelState.IsValid)
            {
                var city = new City
                {
                    CityId = null,
                    Title = newCity.Title,
                    Description = newCity.Description
                };

                _repository.Create(city);
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
        public ActionResult Edit(City newCity)
        {
            City city = _repository.Get((int)newCity.CityId);
            if (ModelState.IsValid)
            {
                city.Title = newCity.Title;
                city.Description = newCity.Description;
                _repository.Update(city);
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