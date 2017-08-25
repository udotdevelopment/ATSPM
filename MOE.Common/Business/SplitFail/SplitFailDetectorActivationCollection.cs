using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailDetectorActivationCollection
    {

        public SortedList<DateTime, SplitFail.SplitFailDetectorActivation> Activations = new SortedList<DateTime, SplitFailDetectorActivation>();

        /// <summary>
        /// 
        /// </summary>
        

        public SplitFailDetectorActivationCollection()
        {



        }



        public void AddActivation(SplitFail.SplitFailDetectorActivation Activation)
        {
            if (this.Activations.ContainsKey(Activation.DetectorOn))
            {
                do
                {
                    Activation.DetectorOn = Activation.DetectorOn.AddSeconds(.01);
                    Activation.DetectorOff = Activation.DetectorOff.AddSeconds(.01);
                } while (this.Activations.ContainsKey(Activation.DetectorOn));

                Activations.Add(Activation.DetectorOn, Activation);
            }
            else
                {
                    Activations.Add(Activation.DetectorOn, Activation);
                }

        }



        public double GreenOccupancy(MOE.Common.Business.CustomReport.Cycle cycle)
        {

            double o = 0;

            
            double t = Convert.ToInt32(cycle.TotalGreenTime * 1000);

            foreach (SplitFailDetectorActivation a in cycle.Activations.Activations.Values)
            {

                o += FindModifiedActivationDuration(cycle.ChangeToGreen, cycle.BeginYellowClear, a);

            }


            double result = division(o, t);
            return result;
        }

        




        public double StartOfRedOccupancy(MOE.Common.Business.CustomReport.Cycle cycle, int SecondsToWatch)
    {
        DateTime EndWatchTime = cycle.ChangeToRed.AddSeconds(SecondsToWatch);
        
        double o = 0;

        foreach (SplitFailDetectorActivation a in cycle.Activations.Activations.Values)
        {

               o += FindModifiedActivationDuration(cycle.ChangeToRed, EndWatchTime, a);


        }

        double t = SecondsToWatch * 1000;


        double result = division(o, t);



        return result;
    }



        public double FindModifiedActivationDuration(DateTime startTime, DateTime endTime, SplitFailDetectorActivation a)
        {
            double d = 0;

            //After start, before end
            if ((a.DetectorOn >= startTime && a.DetectorOn <= endTime) && (a.DetectorOff >= startTime && a.DetectorOff <= endTime))
            {
                d = a.Duration;
            }
            //Before start, before end
            else if ((a.DetectorOn <= startTime && a.DetectorOn <= endTime) && (a.DetectorOff <= endTime && a.DetectorOff >= startTime))
            {


                d = (a.DetectorOff - startTime).TotalMilliseconds;
            }
            //After start, After end
            else if ((a.DetectorOn >= startTime && a.DetectorOn <= endTime) && (a.DetectorOff >= endTime && a.DetectorOff >= startTime))
            {

                d = (endTime - a.DetectorOn).TotalMilliseconds;
            }
            //Before Start, After end
            else if ((a.DetectorOn <= startTime && a.DetectorOn <= endTime) && (a.DetectorOff >= endTime && a.DetectorOff >= startTime))
            {

                d = (endTime - startTime).TotalMilliseconds;
            }
            // 
            else { d = 0; }


            return d;

        }

        private double division(double first, double second)
        {


            if (first > 0 && second > 0)
            {
                double i =  first / second;
                return i;
            }
            else
            {
                return 0;
            }


        }

    }
}
