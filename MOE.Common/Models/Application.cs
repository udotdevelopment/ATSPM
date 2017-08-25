using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{
    public class Application
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<ApplicationSettings> ApplicationSettings { get; set; }
    }
}
