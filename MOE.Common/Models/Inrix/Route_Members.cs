using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models.Inrix
{
    public class Route_Members
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Route_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Segment_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Route_Order { get; set; }
    }
}