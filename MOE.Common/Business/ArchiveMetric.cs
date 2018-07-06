using System;

namespace MOE.Common.Business
{
    public class ArchiveMetric
    {
        //private int binYellowTime;
        //public int BinYellowTime
        //{
        //    get { return binYellowTime; }
        //}

        //private int binRedTime;
        //public int BinRedTime
        //{
        //    get { return binRedTime; }
        //}

        public ArchiveMetric(DateTime timestamp, string detector, int volume, int speed, int delay, int aor,
            int binSize,
            int speedHits, int binGreenTime) //, int binYellowTime, int binRedTime)
        {
            TimeStamp = timestamp;
            Detector = detector;
            Volume = volume;
            Speed = speed;
            Delay = delay;
            Aor = aor;
            BinSize = binSize;
            SpeedHits = speedHits;
            BinGreenTime = binGreenTime;
            //this.binYellowTime = binYellowTime;
            //this.binRedTime = binRedTime;
        }

        public DateTime TimeStamp { get; }

        public string Detector { get; }

        public int Volume { get; }

        public int Speed { get; }

        public int Delay { get; }

        public int Aor { get; }

        public int BinSize { get; }

        public int SpeedHits { get; }

        public int BinGreenTime { get; }
    }
}