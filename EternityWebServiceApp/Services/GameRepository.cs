using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class GameRepository : IRepository<Game>
    {
        private readonly EternityDBContext _context;

        public GameRepository(EternityDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Game> Get()
        {
            return _context.Games.ToList().OrderBy(x => x.GameId);
        }

        public Game Get(int id)
        {
            return _context.Games.FirstOrDefault(x => x.GameId == id);
        }

        public void Create(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public void Update(Game game)
        {
            _context.Games.Update(game);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Games.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
