using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MOE.Common.Models
{
    public class FAQ
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FAQID { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Header { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }
        [Display(Name="Order")]
        public int OrderNumber { get; set; }
    }
}
