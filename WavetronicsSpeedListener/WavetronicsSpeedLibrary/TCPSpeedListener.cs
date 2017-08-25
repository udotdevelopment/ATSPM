using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Diagnostics;
using System.IO;



namespace WavetronicsSpeedLibrary
{

    public class TCPSpeedListener
    {
        private byte[] dataStream = new byte[1024];
        private Socket serverSocket;
        public bool IsRunning { get; set; }
        public TCPSpeedListener()
        {
            IsRunning = true;
        }

        public void StartListening()
        {            
            try
            {   
                // Initialise the IPEndPoint for the clients
                IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
                // Initialise the EndPoint for the clients
                EndPoint epSender = (EndPoint)clients;
                TcpListener listener = new TcpListener(IPAddress.Any, 10088);
                serverSocket = listener.AcceptSocket();
                var childSocketThread = new Thread(() =>
                {
                    byte[] data = new byte[100];
                    serverSocket.BeginReceiveFrom(this.dataStream, 0, this.dataStream.Length, SocketFlags.None,
                    ref epSender, new AsyncCallback(ReceiveData), epSender);

                    serverSocket.Close();
                });
                childSocketThread.Start();
            }
            catch(Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository eventRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                eventRepository.QuickAdd("SpeedListener", this.GetType().ToString(), "StartListening",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
            }
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
            catch(Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository eventRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                eventRepository.QuickAdd("SpeedListener", this.GetType().ToString(), "RecieveData",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
            }
        }
    }
}
