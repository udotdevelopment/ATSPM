using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MOE.Common.Models.Custom
{
    public class SplitFailureSummary
    {
        /// <summary>
        /// Id of the signal
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Signal")]
        public string SignalId { get; set; }

        /// <summary>
        /// Date of the split failure
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Date")]
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
        public int Plan { get; set; }

        /// <summary>
        /// Split fail total
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Split Fail")]
        public double SplitFail { get; set; }

        /// <summary>
        /// Split failure percent
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Split Fail Percent")]
        public double SplitFailPercent { get; set; }
    }
}
