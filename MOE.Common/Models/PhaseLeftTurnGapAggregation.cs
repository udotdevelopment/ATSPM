using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhaseLeftTurnGapAggregation : Aggregation
    {

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [StringLength(10)]
        [Column(Order = 1)]
        public string SignalId { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 3)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 4)]
        public int GapCount1 { get; set; }

        [Required]
        [Column(Order = 5)]
        public int GapCount2 { get; set; }

        [Required]
        [Column(Order = 6)]
        public int GapCount3 { get; set; }


        [Required]
        [Column(Order = 7)]
        public int GapCount4 { get; set; }


        [Required]
        [Column(Order = 8)]
        public int GapCount5 { get; set; }


        [Required]
        [Column(Order = 9)]
        public int GapCount6 { get; set; }


        [Required]
        [Column(Order = 10)]
        public int GapCount7 { get; set; }


        [Required]
        [Column(Order = 11)]
        public int GapCount8 { get; set; }


        [Required]
        [Column(Order = 12)]
        public int GapCount9 { get; set; }


        [Required]
        [Column(Order = 13)]
        public int GapCount10 { get; set; }


        [Required]
        [Column(Order = 14)]
        public int GapCount11 { get; set; }


        [Required]
        [Column(Order = 15)]
        public double SumGapDuration1 { get; set; }

        [Required]
        [Column(Order = 16)]
        public double SumGapDuration2 { get; set; }

        [Required]
        [Column(Order = 17)]
        public double SumGapDuration3 { get; set; }

        [Required]
        [Column(Order = 18)]
        public double SumGreenTime { get; set; }

        public sealed class PhaseLeftTurnGapAggregationClassMap : ClassMap<PhaseLeftTurnGapAggregation>
        {
            public PhaseLeftTurnGapAggregationClassMap()
            {
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.SignalId).Name("Signal Id");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.ApproachId).Name("Approach Id");
                Map(m => m.GapCount1).Name("Gap Count 0 to 1");
                Map(m => m.GapCount2).Name("Gap Count 1 to 3.3");
                Map(m => m.GapCount3).Name("Gap Count 3.3 to 3.7");
                Map(m => m.GapCount4).Name("Gap Count 3.7 to 3.9");
                Map(m => m.GapCount5).Name("Gap Count 3.9 to 4.1");
                Map(m => m.GapCount6).Name("Gap Count 4.1 to 5.3");
                Map(m => m.GapCount7).Name("Gap Count 5.3 to 5.5");
                Map(m => m.GapCount8).Name("Gap Count 5.5 to 6.5");
                Map(m => m.GapCount9).Name("Gap Count 6.5 to 6.9");
                Map(m => m.GapCount10).Name("Gap Count 6.9 to 7.4");
                Map(m => m.GapCount11).Name("Gap Count Over 7.4");
                Map(m => m.SumGapDuration1).Name("Sum Gap Duration Over 4.1");
                Map(m => m.SumGapDuration2).Name("Sum Gap Duration Over 5.3");
                Map(m => m.SumGapDuration3).Name("Sum Gap Duration Over 7.4");
                Map(m => m.SumGreenTime).Name("Sum Green Time");
            }
        }
    }
}