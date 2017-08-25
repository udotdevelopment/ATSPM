using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MOE.Common.Models
{
    public class ApplicationSettings
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }        
        public int ApplicationID { get; set; }
        public virtual Application Application { get; set; }
    }
}
