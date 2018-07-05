using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Models.Inrix
{
    public class Segment
    {
        [Key]
        public int Segment_ID { get; set; }

        [StringLength(50)]
        public string Segment_Name { get; set; }

        [StringLength(50)]
        public string Segment_Description { get; set; }
    }
}