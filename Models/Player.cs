using System;

namespace RPSgame.Models
{
    public class Player
    {
        public enum GameMoves
        {
            rock, paper, scissors
        }

        public Player()
        {
        }

        public Player(string playerName)
        {
            if (playerName == "")
                throw new Exception("Empty player name is not accepted");
            PlayerName = playerName;            
        }


        public string PlayerName { get; set; }

        private string gamemove;
        public string GameMove { 
            get
            { 
                return gamemove; 
            }
            set
            {
                if (value == null || value == GameMoves.paper.ToString() || value == GameMoves.scissors.ToString() || value == GameMoves.rock.ToString())
                {
                    gamemove = value;
                }
                else
                {
                    throw new Exception("Unsupported move");
                } 

            }
        }

    }
}
