using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.ExecutiveReporting
{
    public class ArchiveMetricHour
    {
        double volume;
        public double Volume
        {
            get { return volume; }
        }
        double speed;
        public double Speed
        {
            get { return speed; }
        }
        double delay;
        public double Delay
        {
            get { return delay; }
        }
        double aor;
        public double Aor
        {
            get { return aor; }
        }
        double speedHits;
        public double SpeedHits
        {
            get { return speedHits; }
        }

        private double greenTime;
        public double GreenTime
        {
            get { return greenTime; }
        }

        public bool ValidVolume = true;
        public bool ValidSpeed = true;

        public ArchiveMetricHour(double volume, double speed, double delay, double aor,
            double speedHits, double greenTime)
        {
            this.volume = volume;
            this.speed = speed;
            this.delay = delay;
            this.aor = aor;
            this.speedHits = speedHits;
            this.greenTime = greenTime;
        }


    }
}
