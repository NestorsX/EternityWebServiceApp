using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class DataReferenceController : Controller
    {
        private readonly EternityDBContext _context;
        private readonly IRepository<DataReferenceViewModel> _repository;

        public DataReferenceController(EternityDBContext context, IRepository<DataReferenceViewModel> repository)
        {
            _repository = repository;
            _context = context;
        }

        public ActionResult Index()
        {
            ViewBag.Cities = _context.Cities;
            ViewBag.Attractions = _context.Attractions;
            return View(_repository.Get());
        }

        public ActionResult Create()
        {
            ViewBag.Cities = _context.Cities;
            ViewBag.Attractions = _context.Attractions;
            return View();
        }

        [HttpPost]
        public ActionResult Create(DataReferenceViewModel newDataReference)
        {
            if (_context.DataReferences.FirstOrDefault(x => x.CityId == newDataReference.CityId && x.AttractionId == newDataReference.AttractionId) != null)
            {
                ModelState.AddModelError("AttractionId", "Такая связь уже существует");
            }

            if (ModelState.IsValid)
            {
                _repository.Create(newDataReference);
                return RedirectToAction("Index");
            }

            ViewBag.Cities = _context.Cities;
            ViewBag.Attractions = _context.Attractions;
            return View(newDataReference);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Cities = _context.Cities;
            ViewBag.Attractions = _context.Attractions;
            DataReferenceViewModel dataReference = _repository.Get(id);
            if (dataReference != null)
                return View(dataReference);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(DataReferenceViewModel newDataReference)
        {
            DataReferenceViewModel dataReference = _repository.Get((int)newDataReference.DataReferenceId);
            if (dataReference.CityId == newDataReference.CityId && dataReference.AttractionId == newDataReference.AttractionId)
            {
                ModelState.AddModelError("AttractionId", "Такая связь уже существует");
            }

            if (ModelState.IsValid)
            {
                dataReference.CityId = newDataReference.CityId;
                dataReference.AttractionId = newDataReference.AttractionId;
                _repository.Update(dataReference);
                return RedirectToAction("Index");
            }

            ViewBag.Cities = _context.Cities;
            ViewBag.Attractions = _context.Attractions;
            return View(newDataReference);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            ViewBag.Cities = _context.Cities;
            ViewBag.Attractions = _context.Attractions;
            DataReferenceViewModel dataReference = _repository.Get(id);
            if (dataReference != null)
            {
                return View(dataReference);
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
