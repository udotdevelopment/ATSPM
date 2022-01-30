using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ATSPM.Models
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

        [Display(Name = "Order")]
        public int OrderNumber { get; set; }
    }
}