using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace MOE.Common.Models
{
    public partial class ApplicationEvent
    {
        public ApplicationEvent()
        {

        }

        public enum SeverityLevels
        {
            Information, Low, Medium, High
        }

        [Key]       
        public int ID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string ApplicationName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public SeverityLevels SeverityLevel { get; set; }

        [MaxLength(50)]
        public string Class { get; set; }

        [MaxLength(50)]
        public string Function { get; set; }
    }
}
