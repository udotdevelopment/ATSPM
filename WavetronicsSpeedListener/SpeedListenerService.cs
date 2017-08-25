using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration.Install;

namespace WavetronicsSpeedListener
{
    public partial class SpeedListenerService : ServiceBase
    {
        private Thread th;
        private Thread th2;
        private bool isRunning = false;
        private WavetronicsSpeedLibrary.UDPSpeedListener UDPListener = new WavetronicsSpeedLibrary.UDPSpeedListener();
        private WavetronicsSpeedLibrary.TCPSpeedListener TCPListener = new WavetronicsSpeedLibrary.TCPSpeedListener();
        //private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog eventLog1;
        MOE.Common.Models.Repositories.IApplicationEventRepository eventRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();



        public SpeedListenerService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("SpeedListenerSource")) 
	        {         
			        System.Diagnostics.EventLog.CreateEventSource(
                        "SpeedListenerSource", "SpeedListenerLog");
	        }
            eventLog1.Source = "SpeedListenerSource";
            eventLog1.Log = "SpeedListenerLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");
            isRunning = true;
            th = new Thread(StartUDP);
            th2 = new Thread(StartTCP);
            th.Start();
            th2.Start(); 
        }

        private void StartUDP()
        {
            UDPListener.StartListening();
        }

        private void StartTCP()
        {
            TCPListener.StartListening();
        }

        private void StopListening()
        {           
            eventLog1.WriteEntry("Is Stopping");
            UDPListener.IsRunning = false;
            TCPListener.IsRunning = false;
        }

        protected override void OnStop()
        {
            StopListening();
            isRunning = false;          
            th = null;
            th2 = null;
        }
    }
}
