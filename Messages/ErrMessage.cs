using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPSgame.Messages
{
    public class ErrMessage
    {
        public ErrMessage(string message, string error, Exception exp)
        {
            Message = message;
            Error = error;
            Exp = exp;
        }
        public string Message { get; set; }
        public string Error { get; set; }

        public Exception Exp;
    }
}
