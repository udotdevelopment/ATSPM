using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class Controller_Event_Log
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Controller_Event_Log()
        {
        }

        [Column(Order = 1)]
        [Key]
        [StringLength(10)]
        public string SignalID { get; set; }

        //[Column(Order = 0, TypeName = "datetime2")]
        [Column(Order = 0)]
        [Key]
        public DateTime Timestamp { get; set; }

        [Column(Order = 2)]
        [Key]
        public int EventCode { get; set; }

        [Column(Order = 3)]
        [Key]
        public int EventParam { get; set; }

        public sealed class ControllerEventLogClassMap : ClassMap<Controller_Event_Log>
        {
            public ControllerEventLogClassMap()
            {
                Map(m => m.SignalID).Name("Signal Id");
                Map(m => m.Timestamp).Name("Timestamp");
                Map(m => m.EventCode).Name("Event Code");
                Map(m => m.EventParam).Name("Event Parameter");
            }
        }
    }
}