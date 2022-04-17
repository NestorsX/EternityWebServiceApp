using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly EternityDBContext _context;
        private readonly IRepository<Game> _repository;

        public GameController(EternityDBContext context, IRepository<Game> repository)
        {
            _context = context;
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
        public IActionResult CreateAsync(Game newGame)
        {
            if (_context.Games.FirstOrDefault(x => x.Name == newGame.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая игра уже существует");
            }

            if (ModelState.IsValid)
            {
                var game = new Game
                {
                    GameId = null,
                    Name = newGame.Name
                };

                _repository.Create(game);
                return RedirectToAction("Index");
            }

            return View(newGame);
        }

        public ActionResult Edit(int id)
        {
            Game game = _repository.Get(id);
            if (game != null)
                return View(game);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Game newGame)
        {
            Game game = _repository.Get((int)newGame.GameId);

            if (_context.Games.FirstOrDefault(x => x.Name == newGame.Name && game.Name != newGame.Name) != null)
            {
                ModelState.AddModelError("Name", "Такая игра уже существует");
            }

            if (ModelState.IsValid)
            {
                game.Name = newGame.Name;
                _repository.Update(game);
                return RedirectToAction("Index");
            }

            return View(newGame);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Game game = _repository.Get(id);
            if (game != null)
            {
                return View(game);
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
