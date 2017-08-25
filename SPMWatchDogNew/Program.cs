using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOE.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using Microsoft.AspNet.Identity;
using MOE.Common.Models;
using System.Data.Entity.Validation;
using System.Configuration;




namespace SPMWatchDogNew
{
    class Program
    {



        static void Main(string[] args)
        {
            DateTime StartTime = DateTime.Today;
            // find the analysis timespan
            if (args.Length > 0)
            {

                try
                {
                    StartTime = DateTime.Parse(args[0]);
                    MOE.Common.Business.WatchDog.WatchDogScan scan =
                        new MOE.Common.Business.WatchDog.WatchDogScan(StartTime);
                    scan.StartScan();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "TransferFiles", MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
                }
            }
            else
            {
                try
                {
                    MOE.Common.Business.WatchDog.WatchDogScan scan =
                        new MOE.Common.Business.WatchDog.WatchDogScan(StartTime);
                    scan.StartScan();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    errorRepository.QuickAdd("FTPFromAllcontrollers", "Signal", "TransferFiles", MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
                }
            }

        }
    }
    
}

    


