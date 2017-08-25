using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class ArchiveMetric
    {
        private DateTime timestamp;
        public DateTime TimeStamp
        {
            get { return timestamp; }
        }
        private string detector;
        public string Detector
        {
            get { return detector; }
        }
        private int volume;
        public int Volume
        {
            get { return volume; }
        }
        private int speed;
        public int Speed
        {
            get { return speed; }
        }
        private int delay;
        public int Delay
        {
            get { return delay; }
        }
        private int aor;
        public int Aor
        {
            get { return aor; }
        }
        private int binSize;
        public int BinSize
        {
            get { return binSize; }
        }
        private int speedHits;
        public int SpeedHits
        {
            get { return speedHits; }
        }
        private int binGreenTime;
        public int BinGreenTime
        {
            get { return binGreenTime; }
        }

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

        public ArchiveMetric(DateTime timestamp, string detector, int volume, int speed, int delay, int aor, int binSize,
            int speedHits, int binGreenTime)//, int binYellowTime, int binRedTime)
        {
            this.timestamp = timestamp;
            this.detector = detector;
            this.volume = volume;
            this.speed = speed;
            this.delay = delay;
            this.aor = aor;
            this.binSize = binSize;
            this.speedHits = speedHits;
            this.binGreenTime = binGreenTime;
            //this.binYellowTime = binYellowTime;
            //this.binRedTime = binRedTime;

        }
    }
}
