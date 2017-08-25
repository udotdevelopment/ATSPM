namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agency
    {
        [Key]
        public int AgencyID { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
