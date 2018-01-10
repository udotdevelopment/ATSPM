using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Bins
{
    public class LargeBinsContainer
    {
        public List<BinsContainer> BinsContainers = new List<BinsContainer>();

        public int SumValue
        {
            get
            {
                int sum = 0;

                foreach (var containter in BinsContainers)
                {
                    sum = sum + containter.Bins.Sum(b => b.Value);
                }

                return sum;

            }
        }

        public int BinCount
        {
            get
            {
                int count = 0;

                foreach (var containter in BinsContainers)
                {
                    count = count + containter.Bins.Count;
                }

                return count;
            }
        }

        public int AverageValue
        {
            get
            {
                
                {
                    double sum = 0;
                    double count = 0;

                    foreach (var containter in BinsContainers)
                    {
                        sum = sum + containter.Bins.Sum(b => b.Value);
                        count = count + containter.Bins.Count;
                    }

                    if (sum > count && sum > 0 && count > 0)
                    {
                        return Convert.ToInt32(Math.Round(sum/count));
                    }

                    return 0;
                    
                    

                }
                
            }
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
