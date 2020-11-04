using System;
using static RPSgame.Models.Player;

namespace RPSgame.Models
{
    public class RPSGame
    {
        public enum GameStatus
        {
            NotStarted, WaitingPlayer2, WaitMoves, Finished
        }



        public RPSGame(Player player)
        {
            if (player.PlayerName == null || player.PlayerName == "")
                throw new Exception("Player name cannot be empty or null");
            Id = Guid.NewGuid();
            Winner = "";            
            Player1 = new Player(player.PlayerName);
            Status = GameStatus.WaitingPlayer2;
        }

        public bool IsFinished
        {
            get
            {
                return this.Status == GameStatus.Finished;
            }
        }


        public string GetWinner()
        {
            if (!IsFinished)
                throw new Exception("Game not fininsed");
            if (Winner != "")
                return Winner;

            // Rock beats scissors
            // Scissors beats paper
            // Paper beats rock

            if (Player1.GameMove == Player2.GameMove)
                Winner = "Draw";
            else if (Player1.GameMove == GameMoves.rock.ToString() && Player2.GameMove == GameMoves.scissors.ToString())
                Winner = Player1.PlayerName;
            else if (Player1.GameMove == GameMoves.scissors.ToString() && Player2.GameMove == GameMoves.paper.ToString())
                Winner = Player1.PlayerName;
            else if (Player1.GameMove == GameMoves.paper.ToString() && Player2.GameMove == GameMoves.rock.ToString())
                Winner = Player1.PlayerName;
            else
                Winner = Player2.PlayerName;

            return Winner;

        }
        public void Addplayer(string PlayerName)
        {
            if (PlayerName == "")
                throw new Exception("Game doesn't allow bkank names.");
            if (Player2 != null )
                throw new Exception("Player 2 already joined.");            
            if (PlayerName != Player1.PlayerName)
            { 
                Player2 = new Player(PlayerName);
                Status = GameStatus.WaitMoves;
            }
            else
                throw new Exception("Name already taken.");
        }

        public void MameMove(Player player)
        {
            string move = player.GameMove.ToString().ToLower();

            if (Player1.PlayerName == player.PlayerName)
            {
                if (Player1.GameMove == null)
                    Player1.GameMove = player.GameMove;
                else
                    throw new Exception("Player 1 already made a move.");
            }
            else if (Player2.PlayerName == player.PlayerName)
            {
                if (Player2.GameMove == null)
                    Player2.GameMove = player.GameMove;
                else
                    throw new Exception("Player 2 already made a move.");
            }
            else
                throw new Exception("No such player with name  " +  player.PlayerName + ".");
            if (Player1.GameMove != null && Player2.GameMove != null)
            {
                Status = GameStatus.Finished;
                GetWinner();

            }
        }
        public Guid Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public string Winner { get; set; }
        public GameStatus Status { get; set; }
    }
}
