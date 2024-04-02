using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace MOE.Common.Business.LogDecoder
{
    public class D4Decoder
    {
        public static void DecodeD4File(string fileName, string signalId,
            BlockingCollection<Data.MOE.Controller_Event_LogRow> rowBag, DateTime earliestAcceptableDate)
        {
            var table = new Data.MOE.Controller_Event_LogDataTable();
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader))
            {
                var records = new List<D4Record>();
                try
                {
                    records = csv.GetRecords<D4Record>().ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error reading file {fileName}, trying line-by-line");
                    while (csv.Read())
                    {
                        try
                        {
                            var line = new D4Record
                            {
                                EventCode = csv.GetField<int>(0),
                                Param = csv.GetField<int>(1),
                                DateTime = csv.GetField<DateTime>(2),
                                MsgIdx = csv.GetField<int>(3)
                            };
                            records.Add(line);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }

                foreach (var record in records)
                {
                    if (record.DateTime > earliestAcceptableDate)
                    {
                        try
                        {
                            var row = table.NewController_Event_LogRow();
                            row.Timestamp = record.DateTime;
                            row.EventCode = record.EventCode;
                            row.EventParam = record.Param;
                            row.SignalID = signalId;
                            rowBag.Add(row);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
        }
    }

    public class D4Record
    {
        [Index(0)]
        public int EventCode { get; set; }
        [Index(1)]
        public int Param { get; set; }
        [Index(2)]
        public DateTime DateTime { get; set; }
        [Index(3)]
        public int MsgIdx { get; set; }
    }
}
