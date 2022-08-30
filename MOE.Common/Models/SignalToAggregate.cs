using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    [DataContract]
    public class
        SignalToAggregate
    {
        public SignalToAggregate()
        {
        }

        [StringLength(10)]
        [Required]
        [DataMember]
        [Key]
        public string SignalID { get; set; }

    }
}