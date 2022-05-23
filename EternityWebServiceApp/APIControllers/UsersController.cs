using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.Services;
using System.Text;
using System;

namespace EternityWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly EternityDBContext _context;

        public UsersController(EternityDBContext context)
        {
            _context = context;
        }

        // Получение списка всех пользователей
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        // Получение пользователя по id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        // Получение пользователя по логину и паролю
        [HttpGet("auth/{username}/{password}")]
        public async Task<ActionResult<User>> Get(string username, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username && x.Password == password);
            if (user == null)
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        // Добавление нового пользователя
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (_context.Users.FirstOrDefault(x => x.Email == user.Email) != null)
            {
                return BadRequest("Email уже используется");
            }

            if (_context.Users.FirstOrDefault(x => x.UserName == user.UserName) != null)
            {
                return BadRequest("Никнейм уже используется");
            }

            user.UserId = null;
            user.RoleId = 2;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // Обновление записи пользователя
        [HttpPut]
        public async Task<ActionResult> Put(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (!_context.Users.Any(x => x.UserId == user.UserId))
            {
                return NotFound();
            }

            User currentUser = _context.Users.First(x => x.UserId == user.UserId);
            if (_context.Users.FirstOrDefault(x => x.Email == user.Email && currentUser.Email != user.Email) != null)
            {
                return BadRequest("Email уже используется");
            }

            if (_context.Users.FirstOrDefault(x => x.UserName == user.UserName && currentUser.UserName != user.UserName) != null)
            {
                return BadRequest("Никнейм уже используется");
            }


            currentUser.Email = user.Email;
            currentUser.UserName = user.UserName;
            currentUser.Password = user.Password;
            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Восстановление пароля
        [HttpGet("restorepassword/{email}")]
        public async Task<ActionResult> RestorePassword(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            user.Password = CreateTemporaryPassword();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await EmailService.SendEmailWithTemporaryPasswordAsync(user.Email, user.Password);
            return Ok();
        }

        // Метод генерирует новый временный пароль
        private static string CreateTemporaryPassword()
        {
            var newPassword = new StringBuilder();
            string symbolsAlphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!#$%&()*+-<=>?@[]^_{}~abcdefghijklmnopqrstuvwxyz";
            var rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                newPassword.Append(symbolsAlphabet[rnd.Next(0, symbolsAlphabet.Length - 1)]);
            }

            return newPassword.ToString();
        }

    }
}
