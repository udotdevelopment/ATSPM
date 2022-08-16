using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [DataContract]
    public class DetectorComment : Comment
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public virtual Detector Detector { get; set; }
    }
}