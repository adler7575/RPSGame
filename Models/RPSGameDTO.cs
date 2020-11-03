using System;
using System.Collections.Generic;

namespace RPSgame.Models
{

    public class RPSGameDTO
    {
        public RPSGameDTO(Guid guid)
        {
            Id = guid;
            GameStat = RPSgame.Models.RPSGame.GameStatus.NotStarted.ToString();
            Winner = "To be determined";
        }
        public Guid Id { get; set; }

        public string GameStat { get; set; }

        public string Winner { get; set; }

        public List<Player> lPlayer{ get; set; }
    }
}
