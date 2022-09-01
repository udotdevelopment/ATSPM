using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class ToBeProcessededTable
    {
        public int Id { get; set; }
        public string PartitionedTableName { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string PreserveDataSelect { get; set; }
        public int TableId { get; set; }
        public string PreserveDataWhere { get; set; }
        public string InsertValues { get; set; }
        public string DataBaseName { get; set; }
        public bool Verbose { get; set; }
        public string CreateColumns4Table { get; set; }
    }
}
