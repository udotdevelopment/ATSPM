using System.ComponentModel.DataAnnotations;

namespace ATSPM.Models
{
    public class Agency
    {
        [Key]
        public int AgencyID { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}