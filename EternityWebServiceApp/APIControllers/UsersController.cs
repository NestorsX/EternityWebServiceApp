using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;

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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // Обновление записи пользователя
        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (!_context.Users.Any(x => x.UserId == user.UserId))
            {
                return NotFound();
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
