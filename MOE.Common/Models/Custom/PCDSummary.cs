using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MOE.Common.Models.Custom
{
    public class PCDSummary
    {
        /// <summary>
        /// Id of signal
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Signal")]
        public string SignalId { get; set; }

        /// <summary>
        /// Date of PCD
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
        /// Number of arrivals on green
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Arrival On Green")]
        public double ArrivalOnGreenPercent { get; set; }

        /// <summary>
        /// Total green time
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Green Time")]
        public double GreenTimePercent { get; set; }

        /// <summary>
        /// Platoon ratio
        /// </summary>
        [NotMapped]
        [DataMember(Name = "Platoon Ratio")]
        public double PlatoonRatio { get; set; }
    }
}
