using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EternityWebServiceApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly EternityDBContext _context;
        private readonly IRepository<User> _repository;

        public UserController(EternityDBContext context, IRepository<User> repository)
        {
            _context = context;
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewBag.Roles = _context.Roles;
            return View(_repository.Get());
        }

        public ActionResult Create()
        {
            ViewBag.Roles = _context.Roles;
            return View();
        }

        [HttpPost]
        public IActionResult CreateAsync(User newUser)
        {
            ViewBag.Roles = _context.Roles;
            if (_context.Users.FirstOrDefault(x => x.Email == newUser.Email) != null)
            {
                ModelState.AddModelError("Email", "Email уже используется");
            }

            if (_context.Users.FirstOrDefault(x => x.UserName == newUser.UserName) != null)
            {
                ModelState.AddModelError("Username", "Никнейм уже используется");
            }

            if (newUser.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "Выберите роль");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserId = null,
                    Email = newUser.Email,
                    UserName = newUser.UserName,
                    Password = newUser.Password,
                    RoleId = newUser.RoleId,
                };

                _repository.Create(user);
                return RedirectToAction("Index");
            }

            return View(newUser);
        }

        public ActionResult Edit(int id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index");
            }

            User user = _repository.Get(id);
            if (user != null)
            {
                ViewBag.Roles = _context.Roles;
                ViewData["RoleId"] = user.RoleId;
                return View(user);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(User newUser)
        {
            User user = _repository.Get((int)newUser.UserId);
            if (_context.Users.FirstOrDefault(x => x.Email == newUser.Email && newUser.Email != user.Email) != null)
            {
                ModelState.AddModelError("Email", "Email уже используется");
            }

            if (_context.Users.FirstOrDefault(x => x.UserName == newUser.UserName && newUser.UserName != user.UserName) != null)
            {
                ModelState.AddModelError("Username", "Никнейм уже используется");
            }

            if (newUser.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "Выберите роль");
            }

            if (ModelState.IsValid)
            {
                user.Email = newUser.Email;
                user.UserName = newUser.UserName;
                user.Password = newUser.Password;
                user.RoleId = newUser.RoleId;
                _repository.Update(user);
                return RedirectToAction("Index");
            }

            return View(newUser);
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index");
            }

            User user = _repository.Get(id);
            if (user != null)
            {
                ViewData["RoleName"] = _context.Roles.First(x => x.RoleId == user.RoleId).Name;
                return View(user);
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
