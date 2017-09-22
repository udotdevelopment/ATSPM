namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApproachRouteDetail")]
    public partial class ApproachRouteDetail
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RouteDetailId { get; set; }

        [Required]
        [Display(Name="Route")]
        public int ApproachRouteId { get; set; }
        public virtual ApproachRoute ApproachRoute { get; set; }

        [Required]
        [Display(Name = "Signal Order")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Signal")]     
        public string SignalId { get; set; }

        [Display(Name = "Direction One")]    
        public virtual DirectionType DirectionType1 { get; set; }
  
        [Display(Name = "Direction Two")]       
        public virtual DirectionType DirectionType2 { get; set; }


        [Display(Name = "Phase For Direction One")]
        public int PhaseDirection1 { get; set; }
        public bool IsPhaseDirection1Overlap { get; set; }


        [Display(Name = "Phase For Direction Two")]
        public int PhaseDirection2 { get; set; }
        public bool IsPhaseDirection2Overlap { get; set; }

        [NotMapped]
        public MOE.Common.Models.Signal Signal
        {
            get
            {
                MOE.Common.Models.Repositories.ISignalsRepository sr =
                    MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();

                return sr.GetLatestVersionOfSignalBySignalID(SignalId);

            }

        }

    }
}
