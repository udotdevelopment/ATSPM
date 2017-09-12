using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace MOE.Common.Models.Repositories
{
    public class PreemptAggregationDatasRepository : IPreemptAggregationDatasRepository
    {
        SPM db = new SPM();

        public void Save(PreemptionAggregationData preemptAggregationData)
        {
            try
            {
                db.PreemptionAggregationDatas.Add(preemptAggregationData);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var errorLog = ApplicationEventRepositoryFactory.Create();
                errorLog.QuickAdd(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString(),
                    this.GetType().ToString(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.High, e.Message
                    );
                throw new Exception("Unable to save Preemption Aggragation Data");
            }
        }
    }
}
