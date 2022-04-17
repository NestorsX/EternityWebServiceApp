using EternityWebServiceApp.Models;
using System.Collections.Generic;

namespace EternityWebServiceApp.Interfaces
{
    public interface IGameScoreRepository
    {
        IEnumerable<GameScore> Get();

        GameScore Get(int id);

        void Delete(int id);
    }
}
