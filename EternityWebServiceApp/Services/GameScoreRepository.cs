using EternityWebServiceApp;
using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class GameScoreRepository : IGameScoreRepository
    {
        private readonly EternityDBContext _context;

        public GameScoreRepository(EternityDBContext context)
        {
            _context = context;
        }

        public IEnumerable<GameScore> Get()
        {
            return _context.GameScores.ToList().OrderBy(x => x.GameScoreId);
        }

        public GameScore Get(int id)
        {
            return _context.GameScores.FirstOrDefault(x => x.GameScoreId == id);
        }

        public void Delete(int id)
        {
            _context.GameScores.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
