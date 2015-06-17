using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Data;

namespace Arbetsprov
{
    /// <summary>
    /// Webpage class controls incoming/outgoing data from website.
    /// </summary>
    public class Webpage : Hub
    {
        private DBConnection    DBConn;
        private DBAdapter       DBAdapter;

        /// <summary>
        /// Sends message data to webpage view.
        /// This function also creates and send message data to the database adapter for storing it.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="sender">Sender of the message</param>
        public void Send(string message, string sender)
        {
            DBConn = new DBConnection();
            DBConn.ConnectToDatabase(); //Connect

            DBAdapter = new DBAdapter(DBConn);

            Message msgObject = new Message(0, message, sender);
            DBConn.insert(msgObject);

            Clients.All.broadcastMessage(msgObject.sender, msgObject.text, msgObject.timestamp);
        }

        /// <summary>
        /// Commands database to remove all messages.
        /// </summary>
        public void RemoveMessages()
        {
            DBConn = new DBConnection();
            DBConn.ConnectToDatabase(); //Connect
            DBConn.emptyTable();
            getMessages("ASC");
        }

        /// <summary>
        /// Invokes when a console is connected.
        /// </summary>
        /// <param name="data"></param>
        public void ConsoleActivity(string data)
        {
            Clients.All.consoleActivity(data);
        }

        /// <summary>
        /// Returns all messages to the view. 
        /// This function is called from the webpage in order to recieve existing database message data.
        /// </summary>
        public void getMessages(string order)
        {
            DBConn = new DBConnection();
            DBConn.ConnectToDatabase(); //Connect

            DBAdapter = new DBAdapter(DBConn);

            //Convert message list to json format and return it to "client-side".
            Clients.All.getMessages(DBAdapter.getMessageListAsJSON(order));
        }
    }
}