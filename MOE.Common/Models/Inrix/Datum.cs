using System;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Models.Inrix
{
    public class Datum
    {
        [StringLength(50)]
        public string tmc_code { get; set; }

        public DateTime? measurement_tstamp { get; set; }

        public int? speed { get; set; }

        public int? average_speed { get; set; }

        public int? reference_speed { get; set; }

        public double? travel_time_minutes { get; set; }

        public int? confidence_score { get; set; }

        public int? cvalue { get; set; }

        public int ID { get; set; }
    }
}