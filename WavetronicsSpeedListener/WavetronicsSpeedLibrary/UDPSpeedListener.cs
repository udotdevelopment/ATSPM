using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WavetronicsSpeedLibrary;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Diagnostics;
using System.IO;


namespace WavetronicsSpeedLibrary
{
    public class UDPSpeedListener
    {
        #region Private Members

        // Structure to store the client information
        //private struct Client
        //{
        //    public EndPoint endPoint;
        //    public string name;
        //}
        public bool IsRunning { get; set; }
        // Listing of clients
        private ArrayList clientList;

        // Server socket
        private Socket serverSocket;

        // Data stream
        private byte[] dataStream = new byte[1024];

        // Status delegate
        private delegate void UpdateStatusDelegate(string status);
        private UpdateStatusDelegate updateStatusDelegate = null;

        #endregion
        
            MOE.Common.Models.Repositories.IApplicationEventRepository applicationEventRepository =
                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
        public UDPSpeedListener()
        {
            IsRunning = true;
        }

        public void StartListening()
        {
            //while (IsRunning)
            //{
                try
                {
                    //File.AppendAllText(@"C:\temp\servicenotes.txt", Environment.NewLine + " UDP Listener Is Running " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                    // Initialise the ArrayList of connected clients
                    this.clientList = new ArrayList();

                    // Initialise the socket
                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    // Initialise the IPEndPoint for the server and listen on port 10088
                    IPEndPoint server = new IPEndPoint(IPAddress.Any, 10088);

                    // Associate the socket with this IP address and port
                    serverSocket.Bind(server);

                    // Initialise the IPEndPoint for the clients
                    IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);

                    // Initialise the EndPoint for the clients
                    EndPoint epSender = (EndPoint)clients;

                    // Start listening for incoming data
                        serverSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None,
                            ref epSender, new AsyncCallback(ReceiveData), epSender);
                }
                catch(Exception ex)
            { 
                    applicationEventRepository.QuickAdd("SpeedListener", this.GetType().ToString(), "StartListening",
                        MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
                }
            //}
        }

        private void ReceiveData(IAsyncResult asyncResult)
        {
            try
            {
                string senderIPaddress = asyncResult.AsyncState.ToString();
                
                // Initialise a packet object to store the received data
                Packet receivedData = new Packet(this.dataStream, senderIPaddress);

                // Initialise the IPEndPoint for the clients
                IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);

                // Initialise the EndPoint for the clients
                EndPoint epSender = (EndPoint)clients;

                // Receive all data
                serverSocket.EndReceiveFrom(asyncResult, ref epSender);

                // Listen for more connections again...
                serverSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None, 
                    ref epSender, new AsyncCallback(this.ReceiveData), epSender);

            }
            catch (Exception ex)
            {

                applicationEventRepository.QuickAdd("SpeedListener", this.GetType().ToString(), "RecieveData",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
            }
                
        }
    }
}
