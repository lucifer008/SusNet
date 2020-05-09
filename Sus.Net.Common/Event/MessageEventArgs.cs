using Sus.Net.Common.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sus.Net.Common.Event
{
    public class MessageEventArgs : EventArgs
    {
        public string message { get; set; }

        public MessageEventArgs(string msg)
        {
            this.message = msg;
        }

        public object Tag { set; get; }
        
    }
}
