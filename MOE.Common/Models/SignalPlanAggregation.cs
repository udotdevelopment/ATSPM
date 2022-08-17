using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class SignalPlanAggregation 
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public string SignalId { get; set; }
        
        [Key]
        [Required]
        [Column(Order = 1)]
        public  DateTime Start { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public DateTime End { get; set; }

        [Required]
        [Column(Order = 3)]
        public int PlanNumber { get; set; }
        
        public sealed class
            SignalPlanAggregationClassMap : ClassMap<SignalPlanAggregation>
        {
            public SignalPlanAggregationClassMap()
            {
                Map(m => m.SignalId).Name("Signal ID");
                Map(m => m.Start).Name("Start");
                Map(m => m.End).Name("End");
                Map(m => m.PlanNumber).Name("Plan Number");
            }
        }
    }
}



