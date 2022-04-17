using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class UserRepository : IRepository<User>
    {
        private readonly EternityDBContext _context;

        public UserRepository(EternityDBContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Get()
        {
            return _context.Users.ToList().OrderBy(x => x.UserId);
        }

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(x => x.UserId == id);
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Users.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
