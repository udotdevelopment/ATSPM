using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public partial class SPMWatchDogErrorEvent
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public string SignalID { get; set; }

        public virtual Signal Signal { get; set; }
        
        public string DetectorID { get; set; }
      
        [Required(AllowEmptyStrings=true)]
        public string Direction { get; set; }

        [Required]
        public int Phase { get; set; }

        [Required]
        public int ErrorCode { get; set; }

        [Required]
        public string Message { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            SPMWatchDogErrorEvent y = (SPMWatchDogErrorEvent)obj;
            return this != null && y != null && this.TimeStamp == y.TimeStamp && this.SignalID == y.SignalID
                && this.Phase == y.Phase &&  this.ErrorCode == y.ErrorCode;
        }

        public override int GetHashCode()
        {
            return this == null ? 0 : (this.TimeStamp.GetHashCode() ^ this.SignalID.GetHashCode() ^ this.Phase.GetHashCode());
        }
    }
}
