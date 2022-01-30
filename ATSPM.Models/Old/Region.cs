using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [Table("Region")]
    [DataContract]
    public class Region
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int ID { get; set; }

        [StringLength(50)]
        [DataMember]
        public string Description { get; set; }
    }
}