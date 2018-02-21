using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.UI.DataVisualization.Charting;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PriorityAggregation : Aggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [StringLength(10)]
        public string SignalID { get; set; }

        [ForeignKey("Signal")]
        public int VersionId { get; set; }
        public virtual Signal Signal { get; set; }

        [Required]
        public int PriorityNumber { get; set; }

        [Required]
        public int TotalCycles { get; set; }

        [Required]
        public int PriorityRequests { get; set; }

        [Required]
        public int PriorityServiceEarlyGreen { get; set; }

        [Required]
        public int PriorityServiceExtendedGreen { get; set; }
        [Required]
        public int DataPoints { get; set; }

        public sealed class PriorityAggregationClassMap : ClassMap<PriorityAggregation>
        {
            public PriorityAggregationClassMap()
            {

                Map(m => m.Signal).Ignore();
                Map(m => m.Id).Name("Record Number");
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
