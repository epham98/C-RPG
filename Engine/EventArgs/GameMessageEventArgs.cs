using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EventArgs
{
    public class GameMessageEventArgs : System.EventArgs
    {
        //object to put message to display
        public string Message { get; private set; }

        //constructor sets message value
        public GameMessageEventArgs(string msg)
        {
            Message = msg;
        }
    }
}
