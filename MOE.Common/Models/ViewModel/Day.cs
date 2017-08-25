using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel
{
    public class Day
    {        
        public int DayId { get; set; }
        public string Name { get; set; }

        public Day(int id, string name)
        {
            this.DayId = id;
            this.Name = name;
        }
    }
}
