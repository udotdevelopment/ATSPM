using System.Runtime.CompilerServices;

namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Signal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Signal()
        {

        }

        [NotMapped] private string signalID;

        [Key]
        [Display(Name = "Version")]
        public int VersionID { get; set; }

        [StringLength(10)]
        [Required]
        public string SignalID
        {
            get { return signalID; }
            set
            {
                if (value != signalID)
                {
                    signalID = value;

                    if (this.Approaches != null)
                    {
                        foreach (Approach a in this.Approaches)
                        {
                            a.SignalID = value;
                            if (a.Detectors != null)
                            {
                                foreach (Detector d in a.Detectors)
                                {
                                    d.DetectorID = value + d.DetChannel;
                                }
                            }
                        }
                    }
                }
            }
        }

        [Required]
        [Display(Name = "Version Action")]
        public int VersionActionId { get; set; }
        public virtual VersionAction VersionAction { get; set; }

        [Required]
        [Display(Name = "Version Label")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "First Day This Configuration is Valid")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name="Primary Name")]
        [StringLength(100)]
        public string PrimaryName { get; set; }

        [Required]
        [Display(Name = "Secondary Name")]
        [StringLength(100)]
        public string SecondaryName { get; set; }

        [Required]
        [Display(Name = "IP Address")]
        [StringLength(50)]
        public string IPAddress { get; set; }
        [Required]
        [StringLength(30)]
        public string Latitude { get; set; }

        [Required]
        [StringLength(30)]
        public string Longitude { get; set; }

        [Required]
        [Display(Name = "Region")]
        public int RegionID { get; set; }
        public virtual Region Region { get; set; }

        [Required]
        [Display(Name = "ControllerType Type")]
        public int ControllerTypeID { get; set; }
        public virtual ControllerType ControllerType { get; set; }

        [Required]
        [Display(Name = "Display On Map")]
        public bool Enabled { get; set; }

         [Display(Name = "Chart Notes")]
        public virtual ICollection<MetricComment> Comments { get; set; }


        [Display(Name = "Phase/Direction")]
        public virtual ICollection<Approach> Approaches { get; set; }


    }
}
