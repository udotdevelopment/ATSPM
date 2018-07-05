using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models.Inrix
{
    public class Segment_Members
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Segment_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string TMC { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Segment_Order { get; set; }
    }
}