using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    /// <summary>
    /// This class represent the ConcoleApplication.
    /// </summary>
    class Program
    {
        private HubConnection hubConnection;
        private IHubProxy connection;
        private string website = "http://localhost:63108/"; //This is where website is located.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program application = new Program();
            application.showWelcomeMessage();
            application.connect();
        }

        /// <summary>
        /// Creates the connection with the webpage.
        /// </summary>
        private void connect()
        {
            hubConnection = new HubConnection(website);
            connection = hubConnection.CreateHubProxy("Webpage");

            try
            {
                hubConnection.Start().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Website not available. Run command: 'open-website' to open website.");
                Console.Write("Press any key to exit..");
                Console.ReadLine();
            }

            connection.Invoke("ConsoleActivity", System.Environment.MachineName + " is connected.").Wait();
            commands();
        }

        /// <summary>
        /// Method for console commands.
        /// </summary>
        private void commands()
        {
            string msg = null;
            Console.Write("Command: ");

            while ((msg = Console.ReadLine()) != null)
            {
                switch (msg)
                {
                    case "-help":
                        showHelpMessage();
                        break;
                    case "-send":
                        sendMessage();
                        break;
                    case "-exit":
                        return;
                    case "-open-website":
                        System.Diagnostics.Process.Start(website);
                        Console.WriteLine("Command: Open website.. Done!");
                        break;
                    case "-set-website-path":
                        Console.Write("Website path: ");
                        this.website = Console.ReadLine();
                        Console.WriteLine("Website path: Success.");
                        break;
                    case "-remove-messages":
                        connection.Invoke("RemoveMessages").Wait();
                        Console.WriteLine("Messages removed.");
                        break;
                    default:
                        Console.WriteLine("Unknown command.. Type -help for commands.");
                        break;
                }

                Console.WriteLine("");
                Console.Write("Command: ");
            }
        }

        /// <summary>
        /// Send message method.
        /// </summary>
        private void sendMessage()
        {
            string messageData = null;
            Console.WriteLine("_____________________________________________");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|Type something and press 'enter' to send.   |");
            Console.WriteLine("|Type '-exit' to return.                     |");
            Console.WriteLine("|____________________________________________|");
            Console.WriteLine("");
            Console.Write("Send: ");
            while ((messageData = Console.ReadLine()) != null)
            {
                if (messageData == "-exit")
                {
                    connection.Invoke("ConsoleActivity", System.Environment.MachineName + " is disconnected.").Wait();
                    break;
                }

                connection.Invoke("Send", messageData, System.Environment.MachineName).Wait();
                Console.WriteLine("");
                Console.Write("Send: ");
            }
        }

        /// <summary>
        /// Print all commands.
        /// </summary>
        private void showHelpMessage()
        {
            Console.WriteLine("__________________________________________________________________");
            Console.WriteLine("");
            Console.WriteLine(" Commands");
            Console.WriteLine("");
            Console.WriteLine(" Type -send                    Send messages to  website.");
            Console.WriteLine(" Type -help                    Show help.               ");
            Console.WriteLine(" Type -exit                    Close console application.");
            Console.WriteLine(" Type -open-website            Open website.");
            Console.WriteLine(" Type -set-website-path        Update website url path.");
            Console.WriteLine(" Type -remove-messages         Removes all messages from database.");
            Console.WriteLine("__________________________________________________________________");
            Console.WriteLine("");
        }

        /// <summary>
        /// Show welcome message. Used at start.
        /// </summary>
        private void showWelcomeMessage()
        {
            Console.WriteLine("______________________________________________");
            Console.WriteLine("|                                            |");
            Console.WriteLine("| Console Application by Daniel Theliander   |");
            Console.WriteLine("|____________________________________________|");
            Console.WriteLine("|                                            |");
            Console.WriteLine("| Type -send     Send messages to website.   |");
            Console.WriteLine("| Type -help     Show all commands.          |");
            Console.WriteLine("| Type -exit     Close console application.  |");
            Console.WriteLine("|____________________________________________|");
            Console.WriteLine("");
        }
    }
}