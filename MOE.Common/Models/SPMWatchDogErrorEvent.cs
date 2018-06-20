using System;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Models
{
    public class SPMWatchDogErrorEvent
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public string SignalID { get; set; }


        public string DetectorID { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Direction { get; set; }

        [Required]
        public int Phase { get; set; }

        [Required]
        public int ErrorCode { get; set; }

        [Required]
        public string Message { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var y = (SPMWatchDogErrorEvent) obj;
            return this != null && y != null && TimeStamp == y.TimeStamp && SignalID == y.SignalID
                   && Phase == y.Phase && ErrorCode == y.ErrorCode;
        }

        public override int GetHashCode()
        {
            return this == null ? 0 : TimeStamp.GetHashCode() ^ SignalID.GetHashCode() ^ Phase.GetHashCode();
        }
    }
}