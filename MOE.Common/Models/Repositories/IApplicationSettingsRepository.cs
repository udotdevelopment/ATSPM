using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IApplicationSettingsRepository 
    {
        Models.WatchDogApplicationSettings GetWatchDogSettings();
        void Save(Models.WatchDogApplicationSettings watchDogApplicationSettings);
    }
}
