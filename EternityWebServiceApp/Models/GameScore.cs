using System.ComponentModel.DataAnnotations;

namespace EternityWebServiceApp.Models
{
    public class GameScore
    {
        [Key]
        public int? GameScoreId { get; set; }

        public int UserId { get; set; }

        public int GameId { get; set; }

        public float Score { get; set; }
    }
}
