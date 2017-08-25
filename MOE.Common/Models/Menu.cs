namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MenuId { get; set; }

        [Required]
        [StringLength(50)]
        public string MenuName { get; set; }

        [Required]
        [StringLength(50)]
        public string Controller { get; set; }
        [Required]
        [StringLength(50)]
        public string  Action { get; set; }

        public int ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Application { get; set; }

        public int DisplayOrder { get; set; }
    }
}
