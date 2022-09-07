using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class ToBeProcessededTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string PartitionedTableName { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string PreserveDataSelect { get; set; }

        public int TableId { get; set; }
        [Required]
        public string PreserveDataWhere { get; set; }

        public string InsertValues { get; set; }

        public string DataBaseName { get; set; }

        public bool Verbose { get; set; }
        public string CreateColumns4Table { get; set; }

    }
}
