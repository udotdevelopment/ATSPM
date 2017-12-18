using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PreemptionAggregation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public DateTime BinStartTime { get; set; }

        [Required]
        [StringLength(10)]
        public string SignalId { get; set; }

        [ForeignKey("Signal")]
        public int VersionId { get; set; }
        public virtual Signal Signal { get; set; }

        [Required]
        public int PreemptNumber { get; set; }

        [Required]
        public int PreemptRequests { get; set; }

        [Required]
        public int PreemptServices { get; set; }

        public sealed class PreemptionAggregationClassMap : ClassMap<PreemptionAggregation>
        {
            public PreemptionAggregationClassMap()
            {

                Map(m => m.Signal).Ignore();
                Map(m => m.ID).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.VersionId).Name("Version ID");
                Map(m => m.PreemptNumber).Name("Preempt Number");
                Map(m => m.PreemptRequests).Name("Preempt Requestss");
                Map(m => m.PreemptServices).Name("Preempt Services");
            }
        }
    }
}
