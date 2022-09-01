using System;
using System.ComponentModel.DataAnnotations;

namespace ATSPM.Models
{
    public class ApplicationEvent
    {
        public enum SeverityLevels
        {
            Information,
            Low,
            Medium,
            High
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

        public string Class { get; set; }

        public string Function { get; set; }
    }
}