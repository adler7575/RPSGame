using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSBgame.Models
{
    public enum GameStatus
    {
        NotStarted, NoSuchGame, Started, NoSuchPlayer, PlayerNotUnique, WaitingPlayer2, Player2Joined, WaitMoves, NoSuchMove, PlayerOneMoved, PlayerTwoMoved, PlayerAlreadyMoved, Finshed
    }


    public class RPSGameDTO
    {
        public Guid Id { get; set; }

        public string GameStat { get; set; }

        public string Winner { get; set; }

        public List<Player> lPlayer{ get; set; }
}
}
