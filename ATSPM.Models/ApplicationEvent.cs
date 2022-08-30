using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class ApplicationEvent
    {
        public enum SeverityLevels
        {
            Information,
            Low,
            Medium,
            High
        }
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public int SeverityLevel { get; set; }
        public string Class { get; set; }
        public string Function { get; set; }
        
    }

    
}
