namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Detector
    {
        public Detector()
        {
            MovementTypeID = 1;
            LaneNumber = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        [Required]
        public string DetectorID { get; set; }

        [NotMapped]
        private int detChannel;

        [Required]
        [Display(Name = "Det Channel")]
        public int DetChannel
        {
            get { return detChannel; }
            set
            {
                if (value != detChannel)
                {
                    detChannel = value;
                    if (this.Approach != null && this.Approach.SignalID != null)
                    {
                        DetectorID = this.Approach.SignalID + value;
                    }
                }
            }
        }

        [Display(Name = "Distance To Stop Bar (Advanced Count)")]
        public int? DistanceFromStopBar { get; set; }

        [Display(Name = "Min Speed Filter (Advanced Speed)")]
        public int? MinSpeedFilter { get; set; }
        
        [Required]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Date Disabled")]
        public DateTime? DateDisabled { get; set; }    
        
        public List<int> DetectionTypeIDs { get; set; }
        [Display(Name="Detection Types")]
        public virtual ICollection<DetectionType> DetectionTypes { get; set; }

        [Display(Name = "Lane Number (Lane-by-lane Count)")]
        [Range(0, 50)]
        public int? LaneNumber { get; set; }

        [Display(Name = "Movement Type (Lane-by-lane Count)")]
        public int? MovementTypeID { get; set; }
        public virtual MovementType MovementType { get; set; }

        [Display(Name = "Lane Type (Lane-by-lane Count)")]
        public int? LaneTypeID { get; set; }
        public virtual LaneType LaneType { get; set; }     
    
        [Display(Name = "Decision Point (Advanced Count)")]
        public int? DecisionPoint { get; set; }

        [Display(Name = "Movement Delay (Advanced Speed)")]
        public int? MovementDelay { get; set; }
        
        public List<int> DetectorCommentIDs { get; set; }
        [Display(Name="Detector Comment")]
        public virtual ICollection<DetectorComment> DetectorComments { get; set; }

        public int ApproachID { get; set; }
        public virtual MOE.Common.Models.Approach Approach { get; set; }

        [Display(Name = "Detection Hardware")]
        public int DetectionHardwareID { get; set; }
        [Display(Name = "Detection Hardware")]
        public virtual MOE.Common.Models.DetectionHardware DetectionHardware { get; set; }


    }
}
