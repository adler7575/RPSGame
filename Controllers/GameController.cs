using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RSBgame.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RSBgame.Controllers
{
    [Produces("application/json")]
    [Route("api/RPSGame")]
    [ApiController]
    public class GameController : ControllerBase
    {
        public enum GameMoves 
        { 
            rock , paper, scissors
        }

        public static RSBgame.Models.RPSgame _Game;
        public static RSBgame.Models.RPSGameDTO _GameDTO;
        /// <summary>
        /// Start game as player 1. Rerturns Id to be used later on in joining a game, makeing a move in game, or check game status.
        /// Calling sequence:
        /// Post: api/RPSGame/newgame
        /// body request
        /// {
        //      "PlayerName" : "somename" 
        /// }        
        /// </summary>
        /// <param name="player">Name of player that starts the game/param>
        /// <returns></returns>
        [HttpPost]
        [Route("newgame")]
        public RPSGameDTO NewGame(Player player)
        {
            _Game = new RSBgame.Models.RPSgame();
            _Game.Id = Guid.NewGuid();
            _Game.Player1 = new Player();
            _Game.Player1.PlayerName = player.PlayerName;
            _Game.Player1.GameMove = "";
            _GameDTO = new RSBgame.Models.RPSGameDTO();
            _GameDTO.Id = _Game.Id;
            _GameDTO.GameStat = GameStatus.Started.ToString();
            _GameDTO.Winner = "TBS";
            return _GameDTO;
        }

        /// <summary>
        /// Check status of current game and returns winner if anyone won yet.
        /// Calling sequence:
        /// GET api/RPSGame/{Id}
        /// </summary>
        /// <param name="Id">Guid that defines a game</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public RPSGameDTO GetStatus(Guid Id)
        {
            // Sanity checks
            if (_Game == null)
            {
               _GameDTO.GameStat = GameStatus.NotStarted.ToString();
               return _GameDTO;
            }

            if (Id != _Game.Id)
            {
               _GameDTO.GameStat = GameStatus.NoSuchGame.ToString();
               return _GameDTO;
            }

            if (_Game.Player2 == null)
            {
                _GameDTO.GameStat = GameStatus.Started.ToString();
                return _GameDTO;
            }           
            if (_Game.Player1.GameMove == "" || _Game.Player2.GameMove == "")
            {
                _GameDTO.GameStat = GameStatus.WaitMoves.ToString();
                return _GameDTO;
            }
            _GameDTO.GameStat = GameStatus.Finshed.ToString();
            // Check for draw
            if (_Game.Player1.GameMove == _Game.Player2.GameMove)
            {
                _GameDTO.Winner = "Draw";
                _GameDTO.lPlayer = new List<Player> { _Game.Player1, _Game.Player2 };
                return _GameDTO;
            }
            // Rock wins over scissors
            // Scissors wins over paper
            // Paper wins over rock
            if (_Game.Player1.GameMove == GameMoves.rock.ToString() && _Game.Player2.GameMove == GameMoves.scissors.ToString())
                _GameDTO.Winner = _Game.Player1.PlayerName;            
            else if (_Game.Player1.GameMove == GameMoves.scissors.ToString() && _Game.Player2.GameMove == GameMoves.paper.ToString())
                _GameDTO.Winner = _Game.Player1.PlayerName;
            else if (_Game.Player1.GameMove == GameMoves.paper.ToString() && _Game.Player2.GameMove == GameMoves.rock.ToString())
                _GameDTO.Winner = _Game.Player1.PlayerName;
            else
                _GameDTO.Winner = _Game.Player2.PlayerName;
            _GameDTO.lPlayer = new List<Player> { _Game.Player1, _Game.Player2 };

            return _GameDTO;
        }



        /// <summary>
        /// Join game as player 2. 
        /// Calling sequence:
        /// POST: api/RPSGame/{id}/joingame
        /// body request
        /// {
        //      "PlayerName" : "somename" 
        /// }        
        /// </summary>
        /// <param name="Id">Guid of game to join</param>
        /// <param name="player">Name of second player to join</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/joingame")]
        public object Joingame(Guid Id, Player player)
        {
            // Sanity checks
            if (_Game != null && _Game.Id == Id)
            {
                // Check that the name is not already taken
                if (_Game.Player1.PlayerName == player.PlayerName)
                {
                    _GameDTO.GameStat = GameStatus.PlayerNotUnique.ToString();
                    return _GameDTO;
                }

                _GameDTO.GameStat = GameStatus.Player2Joined.ToString();
                _Game.Player2 = new Player();

                _Game.Player2.PlayerName = player.PlayerName;
                _Game.Player2.GameMove = "";
                return _Game.Player2;
            }
            else
            {
                _GameDTO.GameStat = GameStatus.NoSuchGame.ToString();
            }
            return _GameDTO;
        }

        /// <summary>
        /// Make a move in a game. 
        /// Calling sequence:
        /// POST: api/RPSGame/{id}/makemove
        /// body request
        /// {
        ///     "GameMove" : "some move",
        //      "PlayerName" : "somename" 
        /// }
        /// </summary>
        /// <param name="Id">Guid of game to make a move within</param>
        /// <param name="player">Defines player by name and move to make</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/makemove")]        
        public object Move(Guid Id, Player player)
        {
            // Sanity checks
            if (_Game != null && _Game.Id != Id && _Game.Player1 != null)
            { 
                _GameDTO.GameStat = GameStatus.NotStarted.ToString();
                return _GameDTO;
            }
            // Have player2 joined?
            if (_Game.Player2 == null)
            {
                _GameDTO.GameStat = GameStatus.WaitingPlayer2.ToString();
                return _GameDTO;
            }
            string _move = player.GameMove.ToLower();
            // Sanity check
            if (!(_move == GameMoves.paper.ToString() || _move == GameMoves.scissors.ToString() || _move == GameMoves.rock.ToString()))
            {
                _GameDTO.GameStat = GameStatus.NoSuchMove.ToString();
                return _GameDTO;
            }
                

            if (!(player.PlayerName == _Game.Player1.PlayerName || player.PlayerName == _Game.Player2.PlayerName))
            {
                _GameDTO.GameStat = GameStatus.NoSuchPlayer.ToString();
                return _GameDTO;
            }

            if (player.PlayerName == _Game.Player1.PlayerName)
            {
                if (_Game.Player1.GameMove == "")
                {
                    _GameDTO.GameStat = GameStatus.PlayerOneMoved.ToString();
                    _Game.Player1.GameMove = _move;
                    return _Game.Player1;
                }
                else
                {
                    _GameDTO.GameStat = GameStatus.PlayerAlreadyMoved.ToString();
                }
            }
            else
            {
                if (_Game.Player2.GameMove == "")
                {
                    _GameDTO.GameStat = GameStatus.PlayerTwoMoved.ToString();
                    _Game.Player2.GameMove = _move;
                     return _Game.Player2;
                }
                else
                {
                    _GameDTO.GameStat = GameStatus.PlayerAlreadyMoved.ToString();
                }
            }
            return _GameDTO;
        }

    }
}
