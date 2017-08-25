namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Comment Text")]
        public string CommentText { get; set; }

    }
}
