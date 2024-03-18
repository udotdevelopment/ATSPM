using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MOE.Common.Models.Custom
{
    public class SplitMonitorSummary
    {
        /// <summary>
        /// Id of signal
        /// </summary>
        [NotMapped]
        [DataMember(Name ="Signal")]
        public string SignalId { get; set; }

        /// <summary>
        /// Date of split monitor
        /// </summary>
        [NotMapped]
        [DataMember(Name ="Date")]
        public string Date { get; set; }

        /// <summary>
        /// Phase number
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Phase")]
        public int Phase { get; set; }

        /// <summary>
        /// Plan number
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Plan")]
        public int Plan {  get; set; }

        /// <summary>
        /// Percentile split defaulting to 85 percent
        /// </summary>
        [NotMapped]
        [DataMember(Name = "85th Percentile Split")]
        public double PercentileSplit { get; set; }

        /// <summary>
        /// Average split
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Average Split")]
        public double AverageSplit { get; set; }

        /// <summary>
        /// Number of force offs
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Force Offs")]
        public double ForceOffs { get; set; }

        /// <summary>
        /// Number of gap outs
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Gap Outs")]
        public double GapOuts { get; set; }

        /// <summary>
        /// Number of skips
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Skips")]
        public double Skips { get; set; }
    }
}
