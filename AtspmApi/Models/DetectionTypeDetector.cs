using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AtspmApi.Models
{
    [Table("DetectionTypeDetector")]
    public class DetectionTypeDetector
    {
        [Column(Order = 0)]
        [Key]
        public int ID { get; set; }

        //[Column(Order = 0, TypeName = "datetime2")]
        [Column(Order = 1)]
        [Key]
        public int DetectionTypeID { get; set; }

    }
}