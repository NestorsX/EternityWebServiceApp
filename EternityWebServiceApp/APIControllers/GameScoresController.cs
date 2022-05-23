using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.ViewModels;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameScoresController : ControllerBase
    {
        private readonly EternityDBContext _context;

        public GameScoresController(EternityDBContext context)
        {
            _context = context;
        }

        // Получает лучший результат по игре
        [HttpGet("{id}")]
        public async Task<ActionResult<GameScoreViewModel>> Get(int id)
        {
            Game game = await _context.Games.FirstOrDefaultAsync(x => x.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            GameScore gameScore = await _context.GameScores.FirstOrDefaultAsync(x => x.GameId == game.GameId);
            if (gameScore == null)
            {
                return NotFound();
            }

            gameScore.User = await _context.Users.FirstOrDefaultAsync(x => x.UserId == gameScore.UserId);
            var result = new GameScoreViewModel
            {
                GameName = gameScore.Game.Name,
                UserName = gameScore.User.UserName,
                Score = gameScore.Score,
            };

            return Ok(result);
        }

        // Обновление результата игры
        [HttpPost]
        public async Task<ActionResult> Post(AddGameScoreViewModel newScore)
        {
            GameScore gameScore = await _context.GameScores.FirstOrDefaultAsync(x => x.GameId == newScore.GameId);
            if (gameScore == null)
            {
                gameScore = new GameScore
                {
                    GameScoreId = null,
                    UserId = newScore.UserId,
                    GameId = newScore.GameId,
                    Score = newScore.Score,
                };

                _context.GameScores.Add(gameScore);
                await _context.SaveChangesAsync();
                return Ok();
            }

            string[] bestTime = gameScore.Score.Split(':');
            string[] newTime = newScore.Score.Split(':');
            int.TryParse(bestTime[0], out int bestT);
            int.TryParse(newTime[0], out int newT);
            if (bestT == newT)
            {
                int.TryParse(bestTime[1], out bestT);
                int.TryParse(newTime[1], out newT);
                if (bestT > newT)
                {
                    gameScore.Score = newScore.Score;
                    gameScore.UserId = newScore.UserId;
                    _context.GameScores.Update(gameScore);
                    await _context.SaveChangesAsync();
                    return Ok();
                }

                return Ok();
            }

            if (bestT > newT)
            {
                gameScore.Score = newScore.Score;
                gameScore.UserId = newScore.UserId;
                _context.GameScores.Update(gameScore);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Ok();
        }
    }
}
