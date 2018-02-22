using System.Runtime.Serialization;

namespace MOE.Common.Business.FilterExtensions
{
    [DataContract]
    public class FilterDirection
    {
        public FilterDirection()
        {
        }

        public FilterDirection(int directionDirectionTypeId, string description, bool include)
        {
            DirectionTypeId = directionDirectionTypeId;
            Include = include;
            Description = description;
        }

        [DataMember]
        public int DirectionTypeId { get; set; }

        public string Description { get; set; }

        [DataMember]
        public bool Include { get; set; }
    }
}