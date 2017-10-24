using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models.Custom
{
    public class SignalWithDetection
    {
        [Column(Order=1), Key]
        [StringLength(10)]
        public string SignalID { get; set; }
        public string PrimaryName { get; set; }
        public string Secondary_Name { get; set; }
        public string Latitude { get; set; }
        public string  Longitude { get; set; }
        public string Region { get; set; }
        [Column(Order = 2), Key]
        public int DetectionTypeID { get; set; }
        
    }
}
