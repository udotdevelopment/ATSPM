using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class TablePartitionProcessed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string SwapTableName { get; set; }
        public int PartitionNumber { get; set; }
        public int PartitionBeginYear { get; set; }

        public int PartitionBeginMonth { get; set; }
        [Required]
        public string FileGroupName { get; set; }

        public string PhysicalFileName { get; set; }

        public bool IndexRemoved { get; set; }

        public bool SwappedTableRemoved { get; set; }
        public DateTime TimeIndexdropped { get; set; }
        public DateTime TimeSwappedTableDropped { get; set; }



    }
}
