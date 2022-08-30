using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using AtspmApi.Repositories;

namespace AtspmApi.Models
{
    //[DataContract]
    public class Signal
    {
        [NotMapped] private string signalID;

        [SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Signal()
        {
        }

        [Key]
        [Display(Name = "Version")]
        //[DataMember]
        public int VersionID { get; set; }

        [StringLength(10)]
        [Required]
        //[DataMember]
        public string SignalID
        {
            get => signalID;
            set
            {
                if (value != signalID)
                {
                    signalID = value;

                    if (Approaches != null)
                        foreach (var a in Approaches)
                        {
                            a.SignalID = value;
                            if (a.Detectors != null)
                                foreach (var d in a.Detectors)
                                {
                                    d.DetectorID = value + d.DetChannel.ToString("D2");
                                }
                        }
                }
            }
        }

        [Required]
        [Display(Name = "Version Action")]
        //[DataMember]
        public int VersionActionId { get; set; }

        //[DataMember]
        public virtual VersionAction VersionAction { get; set; }

        [Required]
        [Display(Name = "Version Label")]
        //[DataMember]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Version Start")]
        //[DataMember]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Primary Name")]
        //[DataMember]
        [StringLength(100)]
        public string PrimaryName { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Secondary Name")]
        //[DataMember]
        [StringLength(100)]
        public string SecondaryName { get; set; }

        //[Required]
        //[Display(Name = "IP Address")]
        //[StringLength(50)]
        ////[DataMember]
        //public string IPAddress { get; set; }

        [Required]
        [StringLength(30)]
        //[DataMember]
        public string Latitude { get; set; }

        [Required]
        [StringLength(30)]
        //[DataMember]
        public string Longitude { get; set; }

        [Required]
        [Display(Name = "Region")]
        //[DataMember]
        public int RegionID { get; set; }

        //[DataMember]
        public virtual Region Region { get; set; }

        [Required]
        [Display(Name = "ControllerType Type")]
        //[DataMember]
        public int ControllerTypeID { get; set; }

        ////[DataMember]
        //public virtual ControllerType ControllerType { get; set; }

        [Required]
        [Display(Name = "Display On Map")]
        //[DataMember]
        public bool Enabled { get; set; }

        [Required]
        [Display(Name = "Peds are 1:1")]
        //[DataMember]
        public bool Pedsare1to1 { get; set; }

        //[Display(Name = "Chart Notes")]
        //public virtual ICollection<MetricComment> Comments { get; set; }

        [NotMapped]
        public DateTime FirstDate => Convert.ToDateTime("1/1/2011");


        [Display(Name = "Phase/Direction")]
        [DataMember]
        public virtual ICollection<Approach> Approaches { get; set; }
        public string SignalDescription => SignalID + " - " + PrimaryName + " " + SecondaryName;

        public List<Detector> GetDetectorsForSignal()
        {
            var detectors = new List<Detector>();
            foreach (var a in Approaches.OrderBy(a => a.ProtectedPhaseNumber))
            foreach (var d in a.Detectors)
                detectors.Add(d);
            return detectors.OrderBy(d => d.DetectorID).ToList();
        }
    }
}