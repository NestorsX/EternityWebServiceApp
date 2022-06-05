using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.ViewModels;
using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameScoresController : ControllerBase
    {
        private readonly EternityDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public GameScoresController(EternityDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
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
            string image = null;
            var path = $"{_appEnvironment.WebRootPath}/Images/Users/{gameScore.UserId}";
            if (Directory.Exists(path))
            {
                image = $"{Directory.GetFiles(path).Select(x => Path.GetFileName(x)).First()}";
            }

            var result = new GameScoreViewModel
            {
                GameName = gameScore.Game.Name,
                UserId = gameScore.UserId,
                UserName = gameScore.User.UserName,
                Image = image,
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

            if (newScore.GameId == 1)
            {
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
            }

            if (newScore.GameId == 2)
            {
                if (gameScore.Score == newScore.Score)
                {
                    return Ok();
                }

                if (Convert.ToInt32(gameScore.Score) < Convert.ToInt32(newScore.Score))
                {
                    gameScore.Score = newScore.Score;
                    gameScore.UserId = newScore.UserId;
                    _context.GameScores.Update(gameScore);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }

            return Ok();
        }
    }
}
