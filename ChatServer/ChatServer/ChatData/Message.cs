using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.ChatData
{
    public class Message
    {
        public int Id
        {
            get; set;
        }
        public String Username
        {
            get; set;
        }
        public String UserMessage
        {
            get; set;
        }
    }
}
