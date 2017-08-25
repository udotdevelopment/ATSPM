namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetectorComment:Comment
    {
        public DetectorComment()
        {
        }
     
        public int ID { get; set; }
        public virtual Detector Detector { get; set; }        
    }
}
