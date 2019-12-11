using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AtspmApi.Models
{
    [DataContract]
    public class DetectionHardware
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }
    }
}