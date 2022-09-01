using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [DataContract]
    public partial class Detector
    {
        [NotMapped] [DataMember] private int detChannel;

        public Detector()
        {
            MovementTypeID = 1;
            LaneNumber = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int ID { get; set; }

        [StringLength(50)]
        [Required]
        [DataMember]
        public string DetectorID { get; set; }

        [Required]
        [Display(Name = "Det Channel")]
        [DataMember]
        public int DetChannel
        {
            get => detChannel;
            set
            {
                if (value != detChannel)
                {
                    detChannel = value;
                    if (Approach != null && Approach.SignalID != null)
                    {
                        DetectorID = Approach.SignalID + value.ToString("D2");
                    }
                }
            }
        }

        [Display(Name = "Distance To Stop Bar (Advanced Count)")]
        [DataMember]
        public int? DistanceFromStopBar { get; set; }

        [Display(Name = "Min Speed Filter (Advanced Speed)")]
        [DataMember]
        public int? MinSpeedFilter { get; set; }

        [Required]
        [Display(Name = "Date Added")]
        [DataMember]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Date Disabled")]
        [DataMember]
        public DateTime? DateDisabled { get; set; }

        //public List<int> DetectionTypeIDs { get; set; }

        [Display(Name = "Detection Types")]
        [DataMember]
        public ICollection<DetectionType> DetectionTypes { get; set; }
        [Display(Name = "Lane Number (Lane-by-lane Count)")]
        [DataMember]
        [Range(0, 50)]

        public int? LaneNumber { get; set; }
        [Display(Name = "Movement Type (Lane-by-lane Count)")]
        [DataMember]

        public int? MovementTypeID { get; set; }
        [DataMember]
        public virtual MovementType MovementType { get; set; }

        [Display(Name = "Lane Type (Lane-by-lane Count)")]
        [DataMember]
        public int? LaneTypeID { get; set; }

        [DataMember]
        public virtual LaneType LaneType { get; set; }

        [Display(Name = "Decision Point (Advanced Count)")]
        [DataMember]
        public int? DecisionPoint { get; set; }

        [Display(Name = "Movement Delay (Advanced Speed)")]
        [DataMember]
        public int? MovementDelay { get; set; }

        [Display(Name = "Latency Correction")]
        [DataMember]
        public double LatencyCorrection { get; set; }

        //public List<int> DetectorCommentIDs { get; set; }

        [Display(Name = "Detector Comment")]
        [DataMember]
        public ICollection<DetectorComment> DetectorComments { get; set; }

        [DataMember]
        public int ApproachID { get; set; }

        [DataMember]
        public virtual Approach Approach { get; set; }

        [Display(Name = "Detection Hardware")]
        [DataMember]
        public int DetectionHardwareID { get; set; }

        [Display(Name = "Detection Hardware")]
        [DataMember]
        public virtual DetectionHardware DetectionHardware { get; set; }

        [NotMapped]
        public string Description
        {
            get
            {
                var description = "Channel - " + DetChannel;
                if (LaneNumber != null)
                    description += " Lane Number - " + LaneNumber.Value;
                if (LaneType != null)
                    description += " Lane Type - " + LaneType.Description;
                return description;
            }
        }
    }
}