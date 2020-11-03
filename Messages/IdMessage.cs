using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPSgame.Messages
{
    public class IdMessage
    {
        public IdMessage(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
