using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [DataContract]
    public class DirectionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int DirectionTypeID { get; set; }

        [StringLength(30)]
        [DataMember]
        public string Description { get; set; }

        [StringLength(5)]
        [DataMember]
        public string Abbreviation { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }
    }
}