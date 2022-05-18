using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace AtspmApi.Models
{
    public class ApproachPcdAggregation : Aggregation
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
        [Column(Order = 1)]
        public int ApproachId { get; set; }

        //public virtual Approach Approach { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ArrivalsOnGreen { get; set; }

        [Required]
        [Column(Order = 3)]
        public int ArrivalsOnRed { get; set; }

        [Required]
        [Column(Order = 4)]
        public int ArrivalsOnYellow { get; set; }

        [Key]
        [Required]
        [Column(Order = 5)]
        public bool IsProtectedPhase { get; set; }

        [Required]
        [Column(Order = 6)]
        public int Volume { get; set; }

        public sealed class ApproachPcdAggregationClassMap : ClassMap<ApproachPcdAggregation>
        {
            public ApproachPcdAggregationClassMap()
            {
           //     Map(m => m.Approach).Ignore();
        //        Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.ArrivalsOnRed).Name("Arrivals on Red");
                Map(m => m.ArrivalsOnGreen).Name("Arrivals on Green");
                Map(m => m.ArrivalsOnYellow).Name("Arrivals on Yellow");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
                Map(m => m.Volume).Name("Volume");
            }
        }
    }
}