using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using RPSgame.Messages;
using RPSgame.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RSPgame.Controllers
{
    [Produces("application/json")]
    [Route("api/RPSGame")]
    [ApiController]
    public class GameController : ControllerBase
    {

        public static Dictionary<Guid, RPSGame> gGameList = null;

        public GameController()
        {
            // _GameList will hold all games
            if (gGameList == null)
                gGameList = new Dictionary<Guid, RPSGame>();
            
        }



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
        public object NewGame(Player Player)
        {
            RPSGame Game;
            try 
            {
                Game = new RPSGame(new Player(Player.PlayerName));
                gGameList.Add(Game.Id, Game);
            }
            catch (Exception err)
            {
                ErrMessage errM = new ErrMessage("Something went wrong when creating the game.", err.Message, err);
                return JsonConvert.SerializeObject(BadRequest(errM));
            }

            return Ok(new IdMessage(Game.Id));
            
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
        public object GetStatus(Guid Id)
        {

            try { 
                RPSGame Game;
                Game  = gGameList[Id];
                RPSGameDTO GameDTO = new RPSGameDTO(Game.Id);


                if (Game.IsFinished)
                { 
                    GameDTO.Winner = Game.GetWinner();
                    GameDTO.lPlayer = new List<Player> { Game.Player1, Game.Player2 };
                }
                GameDTO.GameStat = Game.Status.ToString();

                return Ok(GameDTO);
            }
            catch (Exception err)
            {
                ErrMessage errM = new ErrMessage("Something went wrong when quuering status.", err.Message, err);
                return BadRequest(errM);
            }
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
        public object Joingame(Guid Id, Player Player)
        {
            try
            {
                RPSGame Game = gGameList[Id];
                Game.Addplayer(Player.PlayerName);

                RPSGameDTO GameDTO = new RPSGameDTO(Game.Id);
                GameDTO.GameStat = Game.Status.ToString();
                return GameDTO; 
            }
            catch (Exception err)
            {
                ErrMessage errM= new ErrMessage("Something went wrong when player2 tried to join the game.", err.Message, err) ;
                return BadRequest(errM);
            }

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
            try
            {
                RPSGame Game = gGameList[Id];
                Game.MameMove(player);

                RPSGameDTO GameDTO = new RPSGameDTO(Game.Id);
                GameDTO.GameStat = Game.Status.ToString();
                if (Game.IsFinished)
                { 
                    GameDTO.Winner = Game.GetWinner();
                    GameDTO.lPlayer = new List<Player> { Game.Player1, Game.Player2 };
                }
                return GameDTO;
            }
            catch (Exception err)
            {
                ErrMessage errM = new ErrMessage("Something went wrong when player " + player.PlayerName + " tried to make a move.", err.Message, err);
                return BadRequest(errM);
            }

        }

    }
}
