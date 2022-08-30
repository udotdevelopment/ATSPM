using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PriorityAggregation : Aggregation
    {
        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [StringLength(10)]
        [Column(Order = 1)]
        public string SignalID { get; set; }

        [Key]
        [ForeignKey("Signal")]
        [Column(Order = 2)]
        public int VersionId { get; set; }

        [Column(Order = 3)]
        public virtual Signal Signal { get; set; }

        [Required]
        [Column(Order = 4)]
        public int PriorityNumber { get; set; }

        [Required]
        [Column(Order = 5)]
        public int TotalCycles { get; set; }

        [Required]
        [Column(Order = 6)]
        public int PriorityRequests { get; set; }

        [Required]
        [Column(Order = 7)]
        public int PriorityServiceEarlyGreen { get; set; }

        [Required]
        [Column(Order = 8)]
        public int PriorityServiceExtendedGreen { get; set; }
        

        public sealed class PriorityAggregationClassMap : ClassMap<PriorityAggregation>
        {
            public PriorityAggregationClassMap()
            {
                Map(m => m.Signal).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.SignalID).Name("Signal ID");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.VersionId).Name("Version ID");
                Map(m => m.PriorityNumber).Name("Priority Number");
                Map(m => m.TotalCycles).Name("Total Cycles");
                Map(m => m.PriorityRequests).Name("Priority Requests");
                Map(m => m.PriorityServiceEarlyGreen).Name("Priority Service Early Green");
                Map(m => m.PriorityServiceExtendedGreen).Name("Priority Service Extended Green");
            }
        }
    }
}