using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class MetricType : IEqualityComparer<MetricType>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MetricID { get; set; }

        [Required]
        public string ChartName { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [Required]
        public bool ShowOnWebsite { get; set; }

        [Required]
        public bool ShowOnAggregationSite { get; set; }

        //[Required]
        //[Column("DetectionType_DetectionTypeID")]
        //public int DetectionTypeID { get; set; }
        public int DisplayOrder { get; set; }



        public virtual ICollection<DetectionType> DetectionTypes { get; set; }
        public virtual ICollection<MetricComment> Comments { get; set; }
        public virtual ICollection<ActionLog> ActionLogs { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as MetricType);
        }

        public bool Equals(MetricType left, MetricType right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            if (left == null || right == null)
            {
                return false;
            }
            return left.ChartName == right.ChartName && left.Abbreviation == right.Abbreviation;
        }

        public int GetHashCode(MetricType metricType)
        {
            return (metricType.ChartName + metricType.Abbreviation).GetHashCode();
        }
    }


}