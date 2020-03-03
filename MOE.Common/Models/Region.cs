using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    [Table("Regions")]
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