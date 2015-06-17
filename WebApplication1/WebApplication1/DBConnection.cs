using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
 
namespace Arbetsprov
{
    /// <summary>
    /// This class represents a database connection to a local database.
    /// </summary>
    public class DBConnection
    {

        private SqlCeConnection conn;
        private string connString = @"Data Source=|DataDirectory|\Database.sdf"; //Path to local dbfile.

        /// <summary>
        ///Constructor.
        /// </summary>
        public DBConnection()
        {
            //constructor
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~DBConnection()
        {
            //destructor
            conn = null;
        }

        /// <summary>
        /// Close database connection.
        /// </summary>
        public void Disconnect()
        {
            conn.Close();
        }

        /// <summary>
        /// Connect to database
        /// </summary>
        /// <returns> Connected if success else error message.</returns>
        public string ConnectToDatabase()
        {
            try
            {
                conn = new SqlCeConnection(connString);
                conn.Open();
                return "Connected";
            }
            catch (SqlCeException e)
            {
                conn = null;
                return e.Message;
            }
        }

        /// <summary>
        /// Insert into database. 
        /// Currently supports object type of: 
        ///  - Message
        /// </summary>
        /// <param name="item">Object to insert.</param>
        public void insert(Object item)
        {
            if(item.GetType() == typeof(Message))
            {     
                try
                {
                    Message message = (Message) item;
                    string sqlquery = ("INSERT INTO message (text,timestamp,sender)" + 
                                       "Values(@text,@timestamp, @sender)");
                    SqlCeCommand cmd = new SqlCeCommand(sqlquery, conn);
                    cmd.Parameters.AddWithValue("@text", message.text);
                    cmd.Parameters.AddWithValue("@timestamp", message.timestamp);
                    cmd.Parameters.AddWithValue("@sender", message.sender);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Count not insert.");
                }
            }
        }

        /// <summary>
        /// Delete all records in a table.
        /// </summary>
        /// <param name="table"></param>
        public void emptyTable()
        {
            string sqlquery = ("DELETE FROM message");
            SqlCeCommand cmd = new SqlCeCommand(sqlquery, conn);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Return data from database table 'message'.
        /// </summary>
        /// <returns>Returns a data table containing messages.</returns>
        public DataTable getMessageTable()
        {
            DataTable dt = new DataTable();

            try
            {
                string query = "SELECT * FROM message";
                
                SqlCeDataAdapter da = new SqlCeDataAdapter(query, conn);
                da.Fill(dt);
            }
            catch (SqlCeException e){
                //Lazy way...
                DataRow errorRow = dt.NewRow();
                errorRow["Error"] = e.Message;
                errorRow[1] = e.StackTrace;
            }

            return dt;
        }
    }
}