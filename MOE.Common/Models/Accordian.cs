namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Accordian")]
    public partial class Accordian
    {
        [StringLength(150)]
        public string AccHeader { get; set; }

        public string AccContent { get; set; }

        public int? AccOrder { get; set; }

        [StringLength(50)]
        public string Application { get; set; }

        [Key]
        public int AccID { get; set; }
    }
}
