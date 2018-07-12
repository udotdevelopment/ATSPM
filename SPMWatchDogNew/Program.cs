using System;





namespace SPMWatchDogNew
{
    class Program
    {



        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Today;
            // find the analysis timespan
            if (args.Length > 0)
            {

                try
                {
                    startTime = DateTime.Parse(args[0]);
                    MOE.Common.Business.WatchDog.WatchDogScan scan =
                        new MOE.Common.Business.WatchDog.WatchDogScan(startTime);
                    scan.StartScan();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    errorRepository.QuickAdd("SPMWatchdog", "WatchDog", "WatchDogScan with args", MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
                }
            }
            else
            {
                try
                {
                    MOE.Common.Business.WatchDog.WatchDogScan scan =
                        new MOE.Common.Business.WatchDog.WatchDogScan(startTime);
                    scan.StartScan();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository errorRepository = MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    errorRepository.QuickAdd("SPMWatchdog", "WatchDog", "WatchDogScan no args", MOE.Common.Models.ApplicationEvent.SeverityLevels.High, ex.Message);
                }
            }

        }
    }
    
}

    


