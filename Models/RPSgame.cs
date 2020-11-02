using System;
using System.Media;

namespace RSBgame.Models
{
    public class RPSgame
    {
        public Guid Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public bool Finished { get; set; }
        
    }
}
