using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachYellowRedActivationAggregation : Aggregation
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [Column(Order= 1)]
        public int ApproachId { get; set; }

        public virtual Approach Approach { get; set; }

        [Required]
        [Column(Order = 2)]
        public int SevereRedLightViolations { get; set; }

        [Required]
        [Column(Order = 3)]
        public int TotalRedLightViolations { get; set; }

        [Key]
        [Required]
        [Column(Order = 4)]
        public bool IsProtectedPhase { get; set; }

        public sealed class
            ApproachYellowRedActivationAggregationClassMap : ClassMap<ApproachYellowRedActivationAggregation>
        {
            public ApproachYellowRedActivationAggregationClassMap()
            {
                Map(m => m.Approach).Ignore();
                //Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.SevereRedLightViolations).Name("Severe Red Light Violations");
                Map(m => m.TotalRedLightViolations).Name("Total Red Light Violations");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }
    }
}