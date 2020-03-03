using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    [Table("ATSPM_Agencies")]
    public class ATSPM_Agency
    {
        [Key]
        public int AgencyID { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}