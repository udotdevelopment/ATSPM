using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace FTPTimerService
{
    public partial class FTPTimer : ServiceBase
    {
        private AutoResetEvent AutoEventInstance { get; set; }
        private StatusChecker StatusCheckerInstance { get; set; }
        private Timer StateTimer { get; set; }
        public int TimerInterval { get; set; }
        Thread th;

        public FTPTimer()
        {
            InitializeComponent();
            TimerInterval = (Properties.Settings.Default.WaitTimeInSeconds * 1000);
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Launch();
            th = new Thread(StartTimer);
            
        }

        protected override void OnStop()
        {

            StateTimer.Dispose();
            th = null;
        }

        protected void StartTimer()
        {
            AutoEventInstance = new AutoResetEvent(false);
            StatusCheckerInstance = new StatusChecker();

            // Create the delegate that invokes methods for the timer.
            TimerCallback timerDelegate =
                new TimerCallback(StatusCheckerInstance.CheckStatus);

            // Create a timer that signals the delegate to invoke 
            // 1.CheckStatus immediately, 
            // 2.Wait until the job is finished,
            // 3.then wait 5 minutes before executing again. 
            // 4.Repeat from point 2.
           File.AppendAllText(@"C:\temp\servicenotes.txt", Environment.NewLine + " Timer Started " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            //Start Immediately but don't run again.
            StateTimer = new Timer(timerDelegate, AutoEventInstance, 0, Timeout.Infinite);
            while (StateTimer != null)
            {
                //Wait until the job is done
                AutoEventInstance.WaitOne();
                //Wait for before starting the job again.
                StateTimer.Change(TimerInterval, Timeout.Infinite);
            }
           
         }
    }
    class StatusChecker
    {

        public StatusChecker()
        {
        }

        // This method is called by the timer delegate.
        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            File.AppendAllText(@"C:\temp\servicenotes.txt", Environment.NewLine + ("Start Checking status. " + DateTime.Now.ToString("h:mm:ss.fff")));
                
            //This job takes time to run. For example purposes, I put a delay in here.
            int milliseconds = 5000;
            Thread.Sleep(milliseconds);
            //Job is now done running and the timer can now be reset to wait for the next interval
            Console.WriteLine("{0} Done Checking status.",
                DateTime.Now.ToString("h:mm:ss.fff"));
            autoEvent.Set();
        }
    }
}
