using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AtspmApi.Models
{
    [DataContract]
    public class VersionAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}