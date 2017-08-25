namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Program_Settings
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string SettingName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string SettingValue { get; set; }
    }
}
