using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.Common.Business.Tests
{
    [TestClass()]
    public class PlanFactoryTests
    {
        MOE.CommonTests.Models.InMemoryMOEDatabase _db = new InMemoryMOEDatabase();

        [TestInitialize]
        public void Initialize()
        {
            InMemoryApplicationEventRepository appRep = new InMemoryApplicationEventRepository(_db);
            InMemoryControllerEventLogRepository cel = new InMemoryControllerEventLogRepository(_db);


            ControllerEventLogRepositoryFactory.SetRepository(cel);

            ApplicationEventRepositoryFactory.SetApplicationEventRepository(appRep);

        }


        [TestMethod()]
        public void SimplePlanEventsTest()
        {



            //AddEasyPlanEvents(DateTime.Now.AddDays(-1), DateTime.Now, "1001", _db);
            //var plans = PlanFactory.GetBasicPlans(DateTime.Now.AddDays(-1), DateTime.Now, "1001", _db);

            //Assert.IsTrue(plans.Count == 12);
            Assert.IsTrue(false);



        }

        [TestMethod()]
        public void DuplicatePlanEventsTest()
        {



            //AddDuplicatePlanEvents(DateTime.Now.AddDays(-1), DateTime.Now, "1001", _db);
            //var plans = PlanFactory.GetBasicPlans(DateTime.Now.AddDays(-1), DateTime.Now, "1001");

            //Assert.IsTrue(plans.Count == 13);

            //bool adjDupPlans = false;

            //for(int i = 0; i > plans.Count; i++)
            //{
            //    if (plans[i].PlanNumber == plans[i + i].PlanNumber)
            //    {
            //        adjDupPlans = true;
            //    }
            //}

            //Assert.IsFalse(adjDupPlans);
            Assert.IsTrue(false);


        }

        [TestMethod()]
        public void LastTwoPlansAreTheSameShouldOnlyAddTheEarierEventTest()
        {



            //AddDuplicatePlanEvents(DateTime.Now.AddDays(-1), DateTime.Now, "1001", _db);
            //var lastplan = (from r in _db.Controller_Event_Log
            //                where r.EventCode == 131
            //                orderby r.Timestamp
            //                select r).Last();

            //var NextToLastPlan = new Controller_Event_Log();
            //NextToLastPlan.Timestamp = lastplan.Timestamp.AddMinutes(-30);
            //NextToLastPlan.SignalID = lastplan.SignalID;
            //NextToLastPlan.EventCode = lastplan.EventCode;
            //NextToLastPlan.EventParam = lastplan.EventParam;

            //_db.Controller_Event_Log.Add(NextToLastPlan);

            //var plans = PlanFactory.GetBasicPlans(DateTime.Now.AddDays(-1), DateTime.Now, "1001");

            //Assert.IsTrue(plans.Count == 14);

            //bool adjDupPlans = false;

            //for (int i = 0; i > plans.Count; i++)
            //{
            //    if (plans[i].PlanNumber == plans[i + i].PlanNumber)
            //    {
            //        adjDupPlans = true;
            //    }
            //}

            //Assert.IsFalse(adjDupPlans);
            Assert.IsTrue(false);


        }

        private void AddEasyPlanEvents(DateTime start, DateTime end, string signalId, InMemoryMOEDatabase _db)
        {
            int x = 0;
            for (DateTime time = start; time < end; time = time.AddHours(2))
            {
                var e = new Controller_Event_Log();

                e.EventCode = 131;
                e.EventParam = x;
                e.SignalID = signalId;
                e.Timestamp = time;

                x++;
                _db.Controller_Event_Log.Add(e);
            }
        }

        private void AddDuplicatePlanEvents(DateTime start, DateTime end, string signalId, InMemoryMOEDatabase _db)
        {
            var ex = new Controller_Event_Log();

            ex.EventCode = 131;
            ex.EventParam = 10;
            ex.SignalID = signalId;
            ex.Timestamp = start.AddHours(-3);

            _db.Controller_Event_Log.Add(ex);

            int x = 1;
            for (DateTime time = start; time < end; time = time.AddHours(1))
            {
                var e = new Controller_Event_Log();
                e.SignalID = signalId;
                e.Timestamp = time.AddMinutes(30);
                e.EventCode = 131;

                if (x % 2 == 0)
                {
                    e.EventParam = x - 1;
                    
                }
                else
                {
                    e.EventParam = x;
                }
                x++;
                _db.Controller_Event_Log.Add(e);
            }
        }
    }
}