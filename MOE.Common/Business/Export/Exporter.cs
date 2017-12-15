using System.Collections;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace MOE.Common.Business.Export
{
    public class Exporter
    {
        public Exporter(IEnumerable records, string filePath)
        {
            using (StreamWriter writer = File.CreateText("filePath"))
            {
                var csv = new CsvWriter(writer);
                csv.WriteRecords(records);

            }
        }
    }
}