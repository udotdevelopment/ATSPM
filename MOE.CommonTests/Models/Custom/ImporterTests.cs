using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;
using System.Data;
using System.Collections.Concurrent;

namespace MOE.CommonTests.Models.Custom
{
    [TestClass()]
    public class ImporterTests
    {
        private static Random rnd = new Random();

        [TestMethod()]
        public void UniqueConstrainShouldKeepOutDuplicateRecordsTest()
        {

            MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = CreateTableObject(true);

            MOE.Common.Data.MOE.Controller_Event_LogRow eventrow1 = elTable.NewController_Event_LogRow();



            eventrow1.Timestamp = DateTime.Now;
            eventrow1.SignalID = "101";
            eventrow1.EventCode = 1;
            eventrow1.EventParam = 1;
           

            MOE.Common.Data.MOE.Controller_Event_LogRow eventrow2 = elTable.NewController_Event_LogRow();




            eventrow2.Timestamp = eventrow1.Timestamp;
            eventrow2.SignalID = eventrow1.SignalID;
            eventrow2.EventCode = eventrow1.EventCode;
            eventrow2.EventParam = eventrow1.EventParam;

            elTable.AddController_Event_LogRow(eventrow1);

            try
            {
                elTable.AddController_Event_LogRow(eventrow2);
                
            }
            catch
            {
                
            }

            Assert.IsTrue(elTable.Count == 1);

            
 
            


        }

        [TestMethod()]
        public void CopyFromBlockingCollectionIntoConstrainedTableMustNotFailTest()
        {
            var mergedEventsTable = new BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow>();
            MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = CreateTableObject(true);

            MOE.Common.Data.MOE.Controller_Event_LogRow eventrow1 = elTable.NewController_Event_LogRow();
            eventrow1.Timestamp = DateTime.Now;
            eventrow1.SignalID = "101";
            eventrow1.EventCode = 1;
            eventrow1.EventParam = 1;


            MOE.Common.Data.MOE.Controller_Event_LogRow eventrow2 = elTable.NewController_Event_LogRow();
            eventrow2.Timestamp = eventrow1.Timestamp;
            eventrow2.SignalID = eventrow1.SignalID;
            eventrow2.EventCode = eventrow1.EventCode;
            eventrow2.EventParam = eventrow1.EventParam;

            MOE.Common.Data.MOE.Controller_Event_LogRow eventrow3 = elTable.NewController_Event_LogRow();
            eventrow3.Timestamp = DateTime.Now;
            eventrow3.SignalID = "103";
            eventrow3.EventCode = 3;
            eventrow3.EventParam = 3;

            mergedEventsTable.Add(eventrow1);
            mergedEventsTable.Add(eventrow2);
            mergedEventsTable.Add(eventrow3);



            //mergedEventsTable.CopyToDataTable(elTable, LoadOption.PreserveChanges);

            foreach (var r in mergedEventsTable)
            {
                try
                {
                    elTable.AddController_Event_LogRow(r);
                }
                catch { }
            }

            Assert.IsTrue(elTable.Count == 2);
        }

        [TestMethod()]
        public void DoesCopyTakeLongerThanForEach()
        {
            var mergedEventsTable = new BlockingCollection<MOE.Common.Data.MOE.Controller_Event_LogRow>();
            MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = CreateTableObject(true);

            for(int i=0; i < 100000; i++)
            {
                MOE.Common.Data.MOE.Controller_Event_LogRow eventrow1 = elTable.NewController_Event_LogRow();
                eventrow1.Timestamp = DateTime.Now;
                eventrow1.SignalID = "101";
                eventrow1.EventCode = rnd.Next(1, 256);
                eventrow1.EventParam = rnd.Next(1, 256);


                mergedEventsTable.Add(eventrow1);
            }

            DateTime startCopy = DateTime.Now;
            try
            {
                mergedEventsTable.CopyToDataTable(elTable, LoadOption.PreserveChanges);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if(elTable.Count != mergedEventsTable.Count)
            {
                Assert.Fail("The copy method didn't work");
            }

            elTable.Clear();

            DateTime endCopy = DateTime.Now;

            DateTime startForEach = DateTime.Now;
            foreach (var r in mergedEventsTable)
            {
                try
                {
                    elTable.AddController_Event_LogRow(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            if (elTable.Count != mergedEventsTable.Count)
            {
                Assert.Fail("The foreach method didn't work");
            }


            DateTime endForEach = DateTime.Now;

            var copyDiff = (endCopy - startCopy);
            var forEachDiff = (endForEach - startForEach);

            Assert.IsTrue(forEachDiff<copyDiff);


        }

        
 



        private MOE.Common.Data.MOE.Controller_Event_LogDataTable CreateTableObject(bool hasUniqueConstraint)
        {
            MOE.Common.Data.MOE.Controller_Event_LogDataTable elTable = new MOE.Common.Data.MOE.Controller_Event_LogDataTable();

            if (hasUniqueConstraint)
            {
                UniqueConstraint custUnique =
                new UniqueConstraint(new DataColumn[] { elTable.Columns["SignalID"],
                                                    elTable.Columns["Timestamp"],
                                                    elTable.Columns["EventCode"],
                                                    elTable.Columns["EventParam"]
                                  });

                elTable.Constraints.Add(custUnique);
            }

            return elTable;
        }
    }
}
