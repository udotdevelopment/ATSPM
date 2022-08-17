using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class StatusOfProcessedTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string PartitionedTableName { get; set; }
        [Required]
        public DateTime TimeEntered { get; set; }
        public string PartitionName { get; set; }
        public int PartitionYear { get; set; }
        [Required]
        public int PartitionMonth { get; set; }
        public string FunctionOrProcedure { get; set; }
        public string SQLStatementOrMessage { get; set; }
        public string Notes { get; set; }

    }
}
