using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    [Table("Menu")]
    public class Menu
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
        public string Action { get; set; }

        public int ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Application { get; set; }

        public int DisplayOrder { get; set; }
    }
}