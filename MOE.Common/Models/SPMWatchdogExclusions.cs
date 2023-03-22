using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public class SPMWatchdogExclusions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string SignalID { get; set; }

        public int? PhaseID { get; set; }

        [Required]
        public AlertType TypeOfAlert { get; set; }

        [NotMapped]
        public string AlertDescription { get; set; }

        public void SetAlertDescription()
        {
            switch (TypeOfAlert)
            {
                case AlertType.All:
                    AlertDescription = "All Watchdog Reports";
                    break;
                case AlertType.AdvancedDetection:
                    AlertDescription = "Low Advanced Detection Count";
                    break;
                case AlertType.FTP:
                    AlertDescription = "FTP";
                    break;
                case AlertType.ForceOff:
                    AlertDescription = "Force Off";
                    break;
                case AlertType.MaxOut:
                    AlertDescription = "Max Out";
                    break;
                case AlertType.MinimumRecords:
                    AlertDescription = "Minimum Records";
                    break;
                case AlertType.PedActivation:
                    AlertDescription = "High Pedestrian Activation";
                    break;
            }
        }
    }

    public enum AlertType
    {
        [Description("All Reports")]
        All = 0,
        MinimumRecords,
        ForceOff,
        MaxOut,
        AdvancedDetection,
        PedActivation,
        FTP,
    }
}
