using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AtspmApi.Models
{
    [DataContract]
    public class Approach
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Approach()
        {
            //LaneGroups = new HashSet<LaneGroup>();
        }

        [Key]
        [DataMember]
        public int ApproachID { get; set; }

        [StringLength(10)]
        [DataMember]
        public string SignalID { get; set; }

        [Required]
        [DataMember]
        public int VersionID { get; set; }

        [DataMember]
        public virtual Signal Signal { get; set; }

        [Required]
        [Display(Name = "Direction")]
        [DataMember]
        public int DirectionTypeID { get; set; }

        [DataMember]
        public DirectionType DirectionType { get; set; }

        [DataMember]
        public string Description { get;
            set; }

        [Display(Name = "MPH (Advanced Count, Advanced Speed)")]
        [DataMember]
        public int? MPH { get; set; }

        [Required]
        [Display(Name = "Protected Phase")]
        [DataMember]
        public int ProtectedPhaseNumber { get; set; }

        [Display(Name = "Protected Phase Overlap")]
        [DataMember]
        public bool IsProtectedPhaseOverlap { get; set; }

        [Display(Name = "Permissive Phase")]
        [DataMember]
        public int? PermissivePhaseNumber { get; set; }

        [Display(Name = "Perm. Phase Overlap")]
        [DataMember]
        public bool IsPermissivePhaseOverlap { get; set; }

        [Display(Name = "Pedestrian Phase")]
        [DataMember]
        public int? PedestrianPhaseNumber { get; set; }

        [Display(Name = "Ped. Phase Overlap")]
        [DataMember]
        public bool IsPedestrianPhaseOverlap { get; set; }

        [Display(Name = "Ped Detector(s)")]
        [DataMember]
        public string PedestrianDetectors { get; set; }

        [DataMember]
        public virtual ICollection<Detector> Detectors { get; set; }
    }
}