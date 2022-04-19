using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class GameScoreController : Controller
    {
        private readonly EternityDBContext _context;
        private readonly IGameScoreRepository _repository;

        public GameScoreController(EternityDBContext context, IGameScoreRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Get());
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            GameScore gameScore = _repository.Get(id);
            if (gameScore != null)
            {
                return View(gameScore);
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
