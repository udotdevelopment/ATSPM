using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [DataContract]
    public class ControllerType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int ControllerTypeID { get; set; }


        [StringLength(50)]
        [DataMember]
        public string Description { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public long SNMPPort { get; set; }


        [DataMember]
        public string FTPDirectory { get; set; }


        [DataMember]
        public bool ActiveFTP { get; set; }


        [StringLength(50)]
        [DataMember]
        public string UserName { get; set; }


        [StringLength(50)]
        [DataMember]
        public string Password { get; set; }
    }
}