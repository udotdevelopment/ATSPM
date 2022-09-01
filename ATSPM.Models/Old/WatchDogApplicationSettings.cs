using System.ComponentModel.DataAnnotations;

namespace ATSPM.Models
{
    public class WatchDogApplicationSettings : ApplicationSettings
    {
        [Display(Name = "Consecutive Event Count (Max Out and Force Off Alarms)")]
        public int ConsecutiveCount { get; set; }

        [Display(Name = "Min Phase Termination Threshold (Max Out and Force Off Alarms)")]
        public int MinPhaseTerminations { get; set; }

        [Display(Name = "Percent Threshold (Max Out and Force Off Alarms)")]
        public double PercentThreshold { get; set; }

        [Display(Name = "Maximum # of processor threads")]
        public int MaxDegreeOfParallelism { get; set; }

        [Display(Name = "Current Day Evaluation Start Hour (Max Out, Force Off, & Ped Alarms)")]
        public int ScanDayStartHour { get; set; }

        [Display(Name = "Current Day Evaluation End Hour (Max Out, Force Off, & Ped Alarms)")]
        public int ScanDayEndHour { get; set; }

        [Display(Name = "Previous Day Evaluation Start Hour (Low Detector Count Alarm)")]
        public int PreviousDayPMPeakStart { get; set; }

        [Display(Name = "Previous Day Evaluation End Hour (Low Detector Count Alarm)")]
        public int PreviousDayPMPeakEnd { get; set; }

        [Display(Name = "Minimum Record Threshold (Low Detector Count Alarm)")]
        public int MinimumRecords { get; set; }

        [Display(Name = "Weekday Only")]
        public bool WeekdayOnly { get; set; }

        [Display(Name = "Default Email Address")]
        public string DefaultEmailAddress { get; set; }

        [Display(Name = "From Email Address")]
        public string FromEmailAddress { get; set; }

        [Display(Name = "Minimum Count Threshold (Low Detector Count Alarm)")]
        public int LowHitThreshold { get; set; }

        [Display(Name = "Email Server")]
        public string EmailServer { get; set; }

        [Display(Name = "Ped Actuations Threshold (Ped Alarm)")]
        public int MaximumPedestrianEvents { get; set; }

        [Display(Name = "Email All Watch Dog Errors")]
        public bool EmailAllErrors { get; set; }

        public WatchDogApplicationSettings()
        {
            EmailAllErrors = false;
        }
    }
}