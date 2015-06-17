using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Arbetsprov
{
    /// <summary>
    /// DPAdapter is a database adapter and handles the communication between database connection and application.
    /// </summary>
    public class DBAdapter
    {
        private DBConnection DBConn;
        private List<Message> MessageList;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DBConnect">Connection to database</param>
        public DBAdapter(DBConnection DBConnect)
        {
            this.DBConn = DBConnect;
            convertMessageTable();
        }

        /// <summary>
        /// Returns message list in JSON format.
        /// </summary>
        /// <returns>JSON formatted message list</returns>
        public string getMessageListAsJSON(string sort)
        {      
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(getMessageList(sort));
        }

        /// <summary>
        /// Converts data table to List with message objects.
        /// </summary>
        private void convertMessageTable()
        {
            DataTable messageTable = DBConn.getMessageTable();
            List<Message> MessageList = new List<Message>();
            
            foreach (DataRow row in messageTable.Rows)
            {
                string timestamp = row["timestamp"].ToString();
                int id = Convert.ToInt32(row["id"]);
                string text = row["text"].ToString();
                string sender = row["sender"].ToString();

                Message msgObject = new Message(id, text, sender, timestamp);
                MessageList.Add(msgObject);
            }
            setMessageList(MessageList);
        }

        /// <summary>
        /// Returns list containing message objects.
        /// </summary>
        /// <param name="order">Sort order</param>
        /// <returns></returns>
        private List<Message> getMessageList(string order)
        {
            if (order == "DESC")
            {
                return this.MessageList.OrderByDescending(o => o.timestamp).ToList();
            }
            else
            {
                return this.MessageList.OrderBy(o => o.timestamp).ToList();
            }
        }

        /// <summary>
        /// Set list with message objects.
        /// </summary>
        /// <param name="MessageList"></param>
        private void setMessageList(List<Message> MessageList)
        {
            this.MessageList = MessageList;
        }
    }
}