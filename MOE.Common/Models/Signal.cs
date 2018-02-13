using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [DataContract]
    public partial class 
        Signal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Signal()
        {

        }

        [NotMapped] private string signalID;

        [Key]
        [Display(Name = "Version")]
        [DataMember]
        public int VersionID { get; set; }

        [StringLength(10)]
        [Required]
        [DataMember]
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
        [DataMember]
        public int VersionActionId { get; set; }
        [DataMember]
        public virtual VersionAction VersionAction { get; set; }

        [Required]
        [Display(Name = "Version Label")]
        [DataMember]
        public string Note { get; set; }

        [Required]
        [Display(Name = "First Day This Configuration is Valid")]
        [DataMember]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name="Primary Name")]
        [DataMember]
        [StringLength(100)]
        public string PrimaryName { get; set; }

        [Required]
        [Display(Name = "Secondary Name")]
        [DataMember]
        [StringLength(100)]
        public string SecondaryName { get; set; }

        [Required]
        [Display(Name = "IP Address")]
        [StringLength(50)]
        [DataMember]
        public string IPAddress { get; set; }
        [Required]
        [StringLength(30)]
        [DataMember]
        public string Latitude { get; set; }

        [Required]
        [StringLength(30)]
        [DataMember]
        public string Longitude { get; set; }

        [Required]
        [Display(Name = "Region")]
        [DataMember]
        public int RegionID { get; set; }
        [DataMember]
        public virtual Region Region { get; set; }

        [Required]
        [Display(Name = "ControllerType Type")]
        [DataMember]
        public int ControllerTypeID { get; set; }
        [DataMember]
        public virtual ControllerType ControllerType { get; set; }

        [Required]
        [Display(Name = "Display On Map")]
        [DataMember]
        public bool Enabled { get; set; }

         [Display(Name = "Chart Notes")]
        public virtual ICollection<MetricComment> Comments { get; set; }


        [Display(Name = "Phase/Direction")]
        [DataMember]
        public virtual ICollection<Approach> Approaches { get; set; }


    }
}
