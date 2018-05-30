
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public class ShrinkFileGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FileGroupName { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime StartedTimesStamp { get; set; }

        public DateTime CompletedTimeStamp { get; set; }
        [Required]
      
        public bool FileGroupNeedsShrink { get; set; }

        public String Notes { get; set; }

    }
}
