using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arbetsprov
{
    /// <summary>
    /// The class represent a message.
    /// </summary>
    public class Message
    {
        public int          id;
        public string       text;
        public string       timestamp;
        public string       sender;

        /// <summary>
        /// This construct is called when creating a new Message.
        /// </summary>
        /// <param name="id">id of the message</param>
        /// <param name="text">message text</param>
        /// <param name="sender">sender of the message</param>
        public Message(int id, string text, string sender)
        {
            this.id             = id;
            this.text           = text;
            this.sender         = sender;
            this.timestamp = DateTime.Now.ToString("yy-MM-dd H:mm:ss");
        }

        /// <summary>
        /// This construct is called when recreating a Message from database.
        /// Timestamp already exists.
        /// </summary>
        /// <param name="id">id of the message</param>
        /// <param name="text">message text</param>
        /// <param name="sender">sender of the message</param>
        /// <param name="timestamp">time when message sent</param>
        public Message(int id, string text, string sender, string timestamp)
        {
            this.id         = id;
            this.text       = text;
            this.sender     = sender;
            this.timestamp  = timestamp;
        }
    }
}