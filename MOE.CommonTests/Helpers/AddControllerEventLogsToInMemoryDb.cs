using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Helpers
{
    public static class AddControllerEventLogsToInMemoryDb
    {
        public static void LoadControllerEventLogs(InMemoryMOEDatabase inMemoryMoeDatabase)
        {
            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.000"),
                EventParam = 4,
                EventCode = 9
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.000"),
                EventParam = 8,
                EventCode = 9
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.000"),
                EventParam = 4,
                EventCode = 10
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.000"),
                EventParam = 8,
                EventCode = 10
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.000"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.000"),
                EventParam = 47,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.200"),
                EventParam = 47,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.500"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.600"),
                EventParam = 49,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.800"),
                EventParam = 49,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:13.900"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:14.100"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:14.700"),
                EventParam = 46,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 2,
                EventCode = 0
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 6,
                EventCode = 0
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 2,
                EventCode = 1
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 6,
                EventCode = 1
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 4,
                EventCode = 11
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 8,
                EventCode = 11
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 4,
                EventCode = 12
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 8,
                EventCode = 12
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 2,
                EventCode = 21
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 6,
                EventCode = 21
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 2,
                EventCode = 31
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 1,
                EventCode = 32
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 3,
                EventCode = 32
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 1,
                EventCode = 61
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 3,
                EventCode = 61
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 1,
                EventCode = 501
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.000"),
                EventParam = 1,
                EventCode = 502
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 2,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 6,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 1,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 4,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 5,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 8,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 1,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 3,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 4,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 5,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 7,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.100"),
                EventParam = 8,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 1,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 3,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 4,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 5,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 7,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 8,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 1,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 3,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 4,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 5,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 7,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 8,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 1,
                EventCode = 150
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:15.200"),
                EventParam = 128,
                EventCode = 174
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:16.000"),
                EventParam = 45,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:17.100"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:17.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:17.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:17.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:17.600"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.100"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.100"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.200"),
                EventParam = 47,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.400"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.400"),
                EventParam = 47,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.700"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:18.900"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:19.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:19.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:19.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.200"),
                EventParam = 8,
                EventCode = 45
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.200"),
                EventParam = 8,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.400"),
                EventParam = 4,
                EventCode = 45
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.400"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.500"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.600"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.700"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.800"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.800"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.800"),
                EventParam = 8,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:20.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.000"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.100"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.100"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.100"),
                EventParam = 8,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.300"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.400"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.500"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:21.900"),
                EventParam = 8,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.200"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.200"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.300"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.400"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.500"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:22.700"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:23.100"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:23.400"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:23.500"),
                EventParam = 2,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:24.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:24.500"),
                EventParam = 45,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:24.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:24.600"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:24.800"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.000"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.000"),
                EventParam = 5,
                EventCode = 150
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.100"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.200"),
                EventParam = 7,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.200"),
                EventParam = 7,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.400"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.500"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.500"),
                EventParam = 55,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.600"),
                EventParam = 4,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.600"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.700"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.700"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:25.900"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:26.100"),
                EventParam = 2,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:26.200"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:26.300"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:26.300"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:26.500"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:26.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:26.600"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.500"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.500"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.600"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.600"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.700"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.800"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.800"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:27.900"),
                EventParam = 14,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:28.000"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:28.000"),
                EventParam = 14,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:28.000"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:28.100"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:28.600"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:28.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.000"),
                EventParam = 6,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.100"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.400"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.500"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.600"),
                EventParam = 65,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.600"),
                EventParam = 58,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.800"),
                EventParam = 58,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:29.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:30.000"),
                EventParam = 2,
                EventCode = 3
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:30.000"),
                EventParam = 6,
                EventCode = 3
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:30.800"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:30.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.000"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.200"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.200"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.400"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.400"),
                EventParam = 14,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:31.600"),
                EventParam = 14,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:32.100"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:32.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:32.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:32.500"),
                EventParam = 7,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:32.600"),
                EventParam = 66,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:32.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:32.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:33.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:33.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:33.800"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.000"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.100"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.300"),
                EventParam = 7,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:34.500"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:35.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:35.000"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:35.100"),
                EventParam = 2,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:35.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:35.300"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:35.900"),
                EventParam = 14,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.000"),
                EventParam = 14,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.300"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.500"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.600"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.700"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:36.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.100"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.200"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.400"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.400"),
                EventParam = 2,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.400"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.600"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.900"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:37.900"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.000"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.200"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.400"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.400"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.600"),
                EventParam = 5,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.600"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.700"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:38.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:39.200"),
                EventParam = 6,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:39.200"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:39.400"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:39.800"),
                EventParam = 5,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.000"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.200"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.200"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.300"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.300"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.500"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.600"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:40.900"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:41.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:41.300"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:41.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:41.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.000"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.200"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.500"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.600"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.700"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.700"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:42.700"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.200"),
                EventParam = 33,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.400"),
                EventParam = 6,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.400"),
                EventParam = 33,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.400"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.500"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.600"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.700"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:43.800"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.100"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.400"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.500"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.600"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.700"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.800"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.800"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:44.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.000"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.000"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.300"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.500"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.600"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:45.700"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.600"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.900"),
                EventParam = 5,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:46.900"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:47.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:47.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:47.000"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:47.600"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.200"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.300"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.400"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.500"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.500"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.600"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.700"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.700"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.700"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:48.900"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:49.000"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:49.300"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:49.500"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:49.700"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:49.900"),
                EventParam = 7,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.000"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.100"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.100"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.100"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.200"),
                EventParam = 7,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.700"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.800"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:50.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:51.000"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:51.100"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:51.300"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:51.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:51.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:51.500"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:51.900"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.000"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.600"),
                EventParam = 5,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.700"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.800"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.900"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:52.900"),
                EventParam = 6,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.000"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.100"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.300"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.400"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.400"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:53.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:54.200"),
                EventParam = 5,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:54.300"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:54.500"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:54.600"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:54.700"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:54.800"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:54.900"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:55.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:55.200"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:55.300"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:55.400"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.400"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.400"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.600"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.600"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.600"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.600"),
                EventParam = 46,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.700"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.800"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:56.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.400"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.600"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.600"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.700"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.800"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.800"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:57.900"),
                EventParam = 49,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.100"),
                EventParam = 49,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.100"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.200"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.300"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.600"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.600"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.700"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.800"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.800"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.900"),
                EventParam = 5,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.900"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:58.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.000"),
                EventParam = 5,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.100"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.400"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.400"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.600"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.700"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.900"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:08:59.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.000"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.300"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.300"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.300"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.500"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:00.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:01.000"),
                EventParam = 5,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:01.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:01.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:01.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:01.900"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:02.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:02.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:02.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:02.500"),
                EventParam = 33,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:02.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:02.700"),
                EventParam = 33,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:02.700"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:03.100"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:03.400"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:03.400"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:03.800"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:04.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:04.300"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:04.500"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:04.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:04.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:04.900"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:04.900"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.000"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.000"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.000"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.300"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:05.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:06.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:07.000"),
                EventParam = 33,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:07.200"),
                EventParam = 33,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:07.200"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:07.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:07.900"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.300"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.400"),
                EventParam = 14,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.600"),
                EventParam = 14,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.600"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:08.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:09.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:09.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:09.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:09.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:09.600"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:09.800"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:09.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:10.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:10.600"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:10.800"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:10.900"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:10.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:11.000"),
                EventParam = 2,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:11.000"),
                EventParam = 6,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:11.000"),
                EventParam = 2,
                EventCode = 22
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:11.000"),
                EventParam = 6,
                EventCode = 22
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:11.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:11.300"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:12.100"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:12.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:12.400"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:12.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:12.600"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:12.700"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.300"),
                EventParam = 5,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.500"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.800"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:13.900"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.000"),
                EventParam = 5,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.200"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.300"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.400"),
                EventParam = 5,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.400"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:14.400"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:15.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:15.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:15.700"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:15.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:15.900"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.000"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.500"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.600"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.600"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.700"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.700"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.800"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:16.900"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:17.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.000"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.200"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.200"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.400"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.600"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:18.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:19.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:19.200"),
                EventParam = 33,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:19.400"),
                EventParam = 33,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:19.800"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.200"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.300"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.400"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.600"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.600"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.600"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.800"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:20.900"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:21.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:21.200"),
                EventParam = 4,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:21.600"),
                EventParam = 4,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:21.800"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.000"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.600"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.600"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.600"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.700"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:22.800"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:23.100"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:23.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:23.400"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:23.600"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:23.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:23.800"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:23.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:24.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:24.700"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:24.900"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.000"),
                EventParam = 14,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.100"),
                EventParam = 14,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.500"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.500"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.600"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.700"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.700"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:25.800"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.000"),
                EventParam = 2,
                EventCode = 23
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.000"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.100"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:26.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 2,
                EventCode = 6
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 6,
                EventCode = 6
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 2,
                EventCode = 7
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 6,
                EventCode = 7
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 2,
                EventCode = 8
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 6,
                EventCode = 8
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 6,
                EventCode = 23
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 1,
                EventCode = 33
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 3,
                EventCode = 33
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 1,
                EventCode = 63
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 3,
                EventCode = 63
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.000"),
                EventParam = 6,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.100"),
                EventParam = 2,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.100"),
                EventParam = 2,
                EventCode = 151
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.500"),
                EventParam = 5,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.500"),
                EventParam = 6,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.600"),
                EventParam = 2,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.800"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:27.900"),
                EventParam = 5,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:28.000"),
                EventParam = 5,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:28.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:28.200"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:28.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:28.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:29.200"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:29.300"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:29.300"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:29.300"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:29.500"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:29.800"),
                EventParam = 5,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:29.800"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:30.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:30.000"),
                EventParam = 33,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:30.100"),
                EventParam = 3,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:30.100"),
                EventParam = 3,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:30.300"),
                EventParam = 33,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:30.600"),
                EventParam = 1,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:30.700"),
                EventParam = 7,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.000"),
                EventParam = 2,
                EventCode = 9
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.000"),
                EventParam = 6,
                EventCode = 9
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.000"),
                EventParam = 2,
                EventCode = 10
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.000"),
                EventParam = 6,
                EventCode = 10
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.000"),
                EventParam = 1,
                EventCode = 64
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.000"),
                EventParam = 3,
                EventCode = 64
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.100"),
                EventParam = 65,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.600"),
                EventParam = 5,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:31.700"),
                EventParam = 1,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:32.000"),
                EventParam = 7,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:32.100"),
                EventParam = 66,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:32.700"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 4,
                EventCode = 0
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 8,
                EventCode = 0
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 4,
                EventCode = 1
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 8,
                EventCode = 1
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 2,
                EventCode = 11
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 6,
                EventCode = 11
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 2,
                EventCode = 12
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 6,
                EventCode = 12
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 4,
                EventCode = 21
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 8,
                EventCode = 21
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 1,
                EventCode = 31
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 2,
                EventCode = 501
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.000"),
                EventParam = 2,
                EventCode = 502
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 4,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 8,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 300
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 78,
                EventCode = 301
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 302
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 42,
                EventCode = 303
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 304
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 78,
                EventCode = 305
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 306
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 42,
                EventCode = 307
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 308
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 309
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 310
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 311
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 312
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 313
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 314
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.100"),
                EventParam = 0,
                EventCode = 315
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.300"),
                EventParam = 3,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.300"),
                EventParam = 7,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.300"),
                EventParam = 4,
                EventCode = 321
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.300"),
                EventParam = 8,
                EventCode = 321
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.400"),
                EventParam = 0,
                EventCode = 174
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:33.500"),
                EventParam = 54,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:34.300"),
                EventParam = 49,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:34.500"),
                EventParam = 49,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:35.400"),
                EventParam = 33,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:35.600"),
                EventParam = 33,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:35.800"),
                EventParam = 5,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:35.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:36.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:36.100"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:36.300"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:36.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:36.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:36.900"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:37.000"),
                EventParam = 6,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:37.900"),
                EventParam = 46,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.000"),
                EventParam = 4,
                EventCode = 3
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.000"),
                EventParam = 8,
                EventCode = 3
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.000"),
                EventParam = 4,
                EventCode = 22
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.000"),
                EventParam = 8,
                EventCode = 22
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.500"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.700"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.700"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:38.800"),
                EventParam = 54,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:39.500"),
                EventParam = 37,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:39.700"),
                EventParam = 37,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:40.000"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:40.300"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:40.400"),
                EventParam = 54,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:40.800"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:40.900"),
                EventParam = 47,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.000"),
                EventParam = 58,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.100"),
                EventParam = 47,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.200"),
                EventParam = 58,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.300"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.400"),
                EventParam = 57,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.500"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.600"),
                EventParam = 57,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:41.800"),
                EventParam = 54,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:42.000"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.200"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.400"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.500"),
                EventParam = 58,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.700"),
                EventParam = 58,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.800"),
                EventParam = 44,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:43.900"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:44.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:44.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:45.300"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:45.500"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:45.600"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:47.100"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:47.300"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:47.600"),
                EventParam = 58,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:47.800"),
                EventParam = 58,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:48.300"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:48.600"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:48.700"),
                EventParam = 55,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:48.800"),
                EventParam = 4,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:48.800"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:48.900"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:49.000"),
                EventParam = 5,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:49.000"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:50.000"),
                EventParam = 45,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:50.100"),
                EventParam = 8,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:50.300"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:50.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:50.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:51.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:51.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:52.400"),
                EventParam = 55,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:52.500"),
                EventParam = 4,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:53.300"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:53.800"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:54.200"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:54.900"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:55.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:55.300"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:55.400"),
                EventParam = 44,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:55.500"),
                EventParam = 8,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:58.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:58.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.000"),
                EventParam = 37,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.000"),
                EventParam = 58,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.200"),
                EventParam = 58,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.300"),
                EventParam = 37,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.400"),
                EventParam = 6,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.400"),
                EventParam = 33,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.600"),
                EventParam = 33,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.800"),
                EventParam = 55,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:09:59.900"),
                EventParam = 4,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:00.000"),
                EventParam = 120,
                EventCode = 316
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:00.000"),
                EventParam = 25,
                EventCode = 318
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:00.000"),
                EventParam = 0,
                EventCode = 320
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:00.500"),
                EventParam = 45,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:02.000"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:02.200"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:02.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:02.400"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:02.400"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:03.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:03.400"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:03.600"),
                EventParam = 46,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:04.300"),
                EventParam = 49,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:04.500"),
                EventParam = 49,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:04.800"),
                EventParam = 45,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:04.800"),
                EventParam = 47,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:05.000"),
                EventParam = 4,
                EventCode = 23
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:05.000"),
                EventParam = 47,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:05.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:05.400"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:05.400"),
                EventParam = 55,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:05.500"),
                EventParam = 4,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.100"),
                EventParam = 55,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.100"),
                EventParam = 54,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.200"),
                EventParam = 46,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.300"),
                EventParam = 44,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.400"),
                EventParam = 8,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.400"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:06.800"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:07.500"),
                EventParam = 6,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:07.500"),
                EventParam = 57,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:07.700"),
                EventParam = 57,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:07.900"),
                EventParam = 54,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:08.000"),
                EventParam = 4,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:08.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:08.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.000"),
                EventParam = 4,
                EventCode = 4
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.000"),
                EventParam = 8,
                EventCode = 6
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.000"),
                EventParam = 4,
                EventCode = 7
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.000"),
                EventParam = 8,
                EventCode = 7
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.000"),
                EventParam = 4,
                EventCode = 8
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.000"),
                EventParam = 8,
                EventCode = 8
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.000"),
                EventParam = 8,
                EventCode = 23
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.200"),
                EventParam = 44,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:09.300"),
                EventParam = 8,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:10.200"),
                EventParam = 45,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:10.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:10.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:10.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:10.600"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:11.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:11.100"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:11.700"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:11.800"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:11.900"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.000"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.100"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.100"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.200"),
                EventParam = 47,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.400"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.400"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.400"),
                EventParam = 47,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.500"),
                EventParam = 45,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:12.500"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.000"),
                EventParam = 4,
                EventCode = 9
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.000"),
                EventParam = 8,
                EventCode = 9
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.000"),
                EventParam = 4,
                EventCode = 10
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.000"),
                EventParam = 8,
                EventCode = 10
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.300"),
                EventParam = 44,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.400"),
                EventParam = 8,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.500"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.700"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:13.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:14.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:14.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:14.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:14.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:14.700"),
                EventParam = 45,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:14.800"),
                EventParam = 8,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 2,
                EventCode = 0
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 6,
                EventCode = 0
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 2,
                EventCode = 1
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 6,
                EventCode = 1
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 4,
                EventCode = 11
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 8,
                EventCode = 11
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 4,
                EventCode = 12
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 8,
                EventCode = 12
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 2,
                EventCode = 21
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 6,
                EventCode = 21
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 2,
                EventCode = 31
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 1,
                EventCode = 32
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 3,
                EventCode = 32
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 1,
                EventCode = 61
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 3,
                EventCode = 61
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 1,
                EventCode = 501
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.000"),
                EventParam = 1,
                EventCode = 502
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 2,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 6,
                EventCode = 2
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 1,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 4,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 5,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 8,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 1,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 3,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 4,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 5,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 7,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.100"),
                EventParam = 8,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 1,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 3,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 4,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 5,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 7,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 8,
                EventCode = 47
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 1,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 3,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 4,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 5,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 7,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 8,
                EventCode = 49
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 1,
                EventCode = 150
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.200"),
                EventParam = 128,
                EventCode = 174
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.600"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.700"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.800"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:15.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:16.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:16.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:16.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:16.600"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:18.200"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:18.200"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:18.300"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:18.800"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:18.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:18.900"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:18.900"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.100"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.200"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.300"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.300"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.400"),
                EventParam = 19,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.500"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.600"),
                EventParam = 19,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.600"),
                EventParam = 48,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:19.800"),
                EventParam = 48,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.200"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.300"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.400"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.700"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.900"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:20.900"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:21.100"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:21.200"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:21.300"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:21.500"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:21.600"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:21.700"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:21.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:22.600"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:22.800"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:22.800"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:22.800"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:22.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.200"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.400"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.500"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.600"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.700"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.700"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.700"),
                EventParam = 37,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.800"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:23.900"),
                EventParam = 37,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:24.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:24.300"),
                EventParam = 5,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:24.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:24.900"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.000"),
                EventParam = 5,
                EventCode = 150
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.100"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.100"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.200"),
                EventParam = 7,
                EventCode = 46
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.200"),
                EventParam = 7,
                EventCode = 48
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.200"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.400"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:25.900"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:26.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:26.000"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:26.200"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:26.500"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:26.700"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:26.700"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:26.900"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:27.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:27.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:27.600"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:27.800"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:27.900"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:28.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:28.300"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:28.500"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.100"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.300"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.300"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.400"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.800"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.800"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:29.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.000"),
                EventParam = 2,
                EventCode = 3
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.000"),
                EventParam = 6,
                EventCode = 3
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.100"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.300"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.300"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.500"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.600"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.700"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:30.700"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.000"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.100"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.200"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.200"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.400"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.500"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.600"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.700"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.800"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.800"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:31.900"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.300"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.300"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.400"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.500"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.700"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.700"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.900"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:32.900"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.400"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.600"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.600"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.600"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.700"),
                EventParam = 65,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.800"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:33.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:34.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:34.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:34.600"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:35.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:35.200"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:35.300"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:35.600"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:35.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:35.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:35.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:36.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:36.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:36.500"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:36.700"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:36.800"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.000"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.100"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.100"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.200"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.200"),
                EventParam = 5,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.200"),
                EventParam = 6,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.300"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:37.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.000"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.200"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.300"),
                EventParam = 5,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.400"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.500"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.600"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.700"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.800"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.900"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:38.900"),
                EventParam = 37,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.000"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.000"),
                EventParam = 37,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.200"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.200"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.400"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.400"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.400"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.500"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.500"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.600"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:39.700"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:40.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:40.100"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:40.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:40.300"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.000"),
                EventParam = 46,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.100"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.200"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.300"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.400"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.500"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:41.700"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.100"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.300"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.300"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.400"),
                EventParam = 37,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.500"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.600"),
                EventParam = 37,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:42.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:43.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:43.000"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:43.200"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:43.400"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:43.600"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:43.800"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:43.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.000"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.100"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.300"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.900"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:44.900"),
                EventParam = 49,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.000"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.100"),
                EventParam = 49,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.100"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.200"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.300"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.300"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.300"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.500"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:45.900"),
                EventParam = 46,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.000"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.200"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.300"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.400"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.500"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.600"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.700"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.700"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.800"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:46.900"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.100"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.100"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.200"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.200"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.700"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:47.900"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.200"),
                EventParam = 11,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.400"),
                EventParam = 11,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.400"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.400"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.600"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.600"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.800"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.800"),
                EventParam = 54,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.900"),
                EventParam = 4,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.900"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:48.900"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:49.000"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:49.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:49.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:49.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.000"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.100"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.300"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.300"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.400"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.500"),
                EventParam = 5,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.700"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:50.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.100"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.300"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.500"),
                EventParam = 54,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.500"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.500"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.600"),
                EventParam = 4,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.600"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.700"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.700"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:51.800"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:52.000"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:52.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:52.100"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:52.200"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:52.400"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:52.900"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:52.900"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.000"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.100"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.200"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.300"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.800"),
                EventParam = 5,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:53.800"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.000"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.300"),
                EventParam = 17,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.300"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.500"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.600"),
                EventParam = 8,
                EventCode = 45
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.600"),
                EventParam = 8,
                EventCode = 90
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.700"),
                EventParam = 12,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.900"),
                EventParam = 1,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.900"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:54.900"),
                EventParam = 12,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.000"),
                EventParam = 5,
                EventCode = 44
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.000"),
                EventParam = 1,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.100"),
                EventParam = 8,
                EventCode = 89
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.200"),
                EventParam = 18,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.300"),
                EventParam = 18,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.400"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.600"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.600"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.800"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.900"),
                EventParam = 3,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:55.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:56.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:56.400"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:56.500"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:56.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:56.700"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.400"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.500"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.500"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.600"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.600"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.700"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.700"),
                EventParam = 13,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.700"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.900"),
                EventParam = 13,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:57.900"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:58.000"),
                EventParam = 3,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:58.100"),
                EventParam = 5,
                EventCode = 43
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:58.600"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:58.800"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.000"),
                EventParam = 35,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.200"),
                EventParam = 35,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.200"),
                EventParam = 34,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.400"),
                EventParam = 34,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.600"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.700"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.800"),
                EventParam = 36,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:10:59.900"),
                EventParam = 20,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:11:00.000"),
                EventParam = 20,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:11:00.000"),
                EventParam = 36,
                EventCode = 81
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:11:00.000"),
                EventParam = 17,
                EventCode = 82
            });

            inMemoryMoeDatabase.Controller_Event_Log.Add(new MOE.Common.Models.Controller_Event_Log
            {
                SignalID = "7185",
                Timestamp = Convert.ToDateTime("2017-10-17 17:11:00.700"),
                EventParam = 44,
                EventCode = 82
            });


        }
    }
}
