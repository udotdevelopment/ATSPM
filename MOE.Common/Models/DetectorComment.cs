using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [DataContract]
    public partial class DetectorComment:Comment
    {
        public DetectorComment()
        {
        }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public virtual Detector Detector { get; set; }        
    }
}
