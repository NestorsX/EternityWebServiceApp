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
            IEnumerable<GameScore> gameScores = _context.GameScores.ToList().OrderBy(x => x.GameScoreId);
            foreach (GameScore gameScore in gameScores)
            {
                gameScore.User = _context.Users.FirstOrDefault(x => x.UserId == gameScore.UserId);
                gameScore.Game = _context.Games.FirstOrDefault(x => x.GameId == gameScore.GameId);
            }

            return gameScores;
        }

        public GameScore Get(int id)
        {
            GameScore gameScore = _context.GameScores.FirstOrDefault(x => x.GameScoreId == id);
            gameScore.User = _context.Users.FirstOrDefault(x => x.UserId == gameScore.UserId);
            gameScore.Game = _context.Games.FirstOrDefault(x => x.GameId == gameScore.GameId);
            return gameScore;
        }

        public void Delete(int id)
        {
            _context.GameScores.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
