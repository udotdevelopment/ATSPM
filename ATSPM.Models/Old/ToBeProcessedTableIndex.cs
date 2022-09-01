using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class ToBeProcessedTableIndex
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int TableId { get; set; }
        public int IndexId { get; set; }
        public string ClusteredText { get; set; }
        public string TextForIndex { get; set; }
        public string IndexName { get; set; }
    }
}
