namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Alert_Day_Types
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DayTypeNumber { get; set; }

        [StringLength(50)]
        public string DayTypeDesctiption { get; set; }
    }
}
