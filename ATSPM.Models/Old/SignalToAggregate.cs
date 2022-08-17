using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [DataContract]
    public class
        SignalToAggregate
    {
        public SignalToAggregate()
        {
        }

        [StringLength(10)]
        [Required]
        [DataMember]
        [Key]
        public string SignalID { get; set; }

    }
}