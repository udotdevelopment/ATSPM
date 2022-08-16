using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [DataContract]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int CommentID { get; set; }

        [Required]
        [DataMember]
        public DateTime TimeStamp { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Comment Text")]
        [DataMember]
        public string CommentText { get; set; }
    }
}