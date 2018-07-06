using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public class DatabaseArchiveExcludedSignal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string SignalId { get; set; }
        [NotMapped]
        public string SignalDescription { get; set; }
    }
}