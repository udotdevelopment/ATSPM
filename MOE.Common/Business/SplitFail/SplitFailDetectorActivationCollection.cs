using System;
using System.Collections.Generic;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailDetectorActivationCollection
    {
        public SortedList<DateTime, SplitFailDetectorActivation> Activations =
            new SortedList<DateTime, SplitFailDetectorActivation>();

        public void AddActivation(SplitFailDetectorActivation Activation)
        {
            if (Activations.ContainsKey(Activation.DetectorOn))
            {
                do
                {
                    Activation.DetectorOn = Activation.DetectorOn.AddSeconds(.01);
                    Activation.DetectorOff = Activation.DetectorOff.AddSeconds(.01);
                } while (Activations.ContainsKey(Activation.DetectorOn));

                Activations.Add(Activation.DetectorOn, Activation);
            }
            else
            {
                Activations.Add(Activation.DetectorOn, Activation);
            }
        }


        //public double StartOfRedOccupancy(CycleSplitFail cycle, int secondsToWatch)
        //{
        //    DateTime endWatchTime = cycle.EndTime.AddSeconds(secondsToWatch);
        //    double o = 0;
        //    foreach (SplitFailDetectorActivation a in cycle.Activations.Activations.Values)
        //    {
        //           o += FindModifiedActivationDuration(cycle.EndTime, endWatchTime, a);
        //    }
        //    double t = secondsToWatch * 1000;
        //    double result = division(o, t);
        //    return result;
        //}

        //public double FindModifiedActivationDuration(DateTime startTime, DateTime endTime, SplitFailDetectorActivation a)
        //{
        //    double d = 0;
        //    //After start, before end
        //    if ((a.DetectorOn >= startTime && a.DetectorOn <= endTime) && (a.DetectorOff >= startTime && a.DetectorOff <= endTime))
        //    {
        //        d = a.Duration;
        //    }
        //    //Before start, before end
        //    else if ((a.DetectorOn <= startTime && a.DetectorOn <= endTime) && (a.DetectorOff <= endTime && a.DetectorOff >= startTime))
        //    {
        //        d = (a.DetectorOff - startTime).TotalMilliseconds;
        //    }
        //    //After start, After end
        //    else if ((a.DetectorOn >= startTime && a.DetectorOn <= endTime) && (a.DetectorOff >= endTime && a.DetectorOff >= startTime))
        //    {
        //        d = (endTime - a.DetectorOn).TotalMilliseconds;
        //    }
        //    //Before Start, After end
        //    else if ((a.DetectorOn <= startTime && a.DetectorOn <= endTime) && (a.DetectorOff >= endTime && a.DetectorOff >= startTime))
        //    {
        //        d = (endTime - startTime).TotalMilliseconds;
        //    }
        //    // 
        //    else { d = 0; }
        //    return d;
        //}

        //private double division(double first, double second)
        //{
        //    if (first > 0 && second > 0)
        //    {
        //        double i =  first / second;
        //        return i;
        //    }
        //    return 0;
        //}
    }
}