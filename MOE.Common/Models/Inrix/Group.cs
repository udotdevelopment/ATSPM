using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Models.Inrix
{
    public class Group
    {
        [Key]
        public int Group_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Group_Name { get; set; }

        public string Group_Description { get; set; }
    }
}