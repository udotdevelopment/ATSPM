namespace MOE.Common.Business.Parquet
{
    public class ParquetEventLog
    {
        public string SignalID { get; set; }
        public string Date { get; set; }
        public double TimestampMs { get; set; }
        public int EventCode { get; set; }
        public int EventParam { get; set; }

        public ParquetEventLog()
        { }
    }
}
