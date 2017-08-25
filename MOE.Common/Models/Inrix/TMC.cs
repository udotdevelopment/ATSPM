namespace MOE.Common.Models.Inrix
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TMC")]
    public partial class TMC
    {
        [Key]
        [Column("TMC")]
        [StringLength(50)]
        public string TMC1 { get; set; }

        [Required]
        [StringLength(50)]
        public string Direction { get; set; }

        [Required]
        public string TMC_Start { get; set; }

        [Required]
        [StringLength(50)]
        public string TMC_Stop { get; set; }

        public decimal Length { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }
    }
}
