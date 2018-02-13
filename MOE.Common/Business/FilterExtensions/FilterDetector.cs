using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.FilterExtensions
{
    [DataContract]
    public class FilterDetector
    {
        [DataMember]
        public int Id { get; set; }

        public string Description { get; set; }
        [DataMember]
        public bool Exclude { get; set; } = false;


    }
}
