using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class ApplicationSetting
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int? ConsecutiveCount { get; set; }
        public int? MinPhaseTerminations { get; set; }
        public double? PercentThreshold { get; set; }
        public int? MaxDegreeOfParallelism { get; set; }
        public int? ScanDayStartHour { get; set; }
        public int? ScanDayEndHour { get; set; }
        public int? PreviousDayPmpeakStart { get; set; }
        public int? PreviousDayPmpeakEnd { get; set; }
        public int? MinimumRecords { get; set; }
        public bool? WeekdayOnly { get; set; }
        public string DefaultEmailAddress { get; set; }
        public string FromEmailAddress { get; set; }
        public int? LowHitThreshold { get; set; }
        public string EmailServer { get; set; }
        public int? MaximumPedestrianEvents { get; set; }
        public string Discriminator { get; set; }
        public string ArchivePath { get; set; }
        public int? SelectedDeleteOrMove { get; set; }
        public int? NumberOfRows { get; set; }
        public int? StartTime { get; set; }
        public int? TimeDuration { get; set; }
        public string ImageUrl { get; set; }
        public string ImagePath { get; set; }
        public int? RawDataCountLimit { get; set; }
        public string ReCaptchaPublicKey { get; set; }
        public string ReCaptchaSecretKey { get; set; }
        public bool? EnableDatbaseArchive { get; set; }
        public int? SelectedTableScheme { get; set; }
        public int? MonthsToKeepIndex { get; set; }
        public int? MonthsToKeepData { get; set; }
        public bool? EmailAllErrors { get; set; }
        public int? CycleCompletionSeconds { get; set; }

        public virtual Application Application { get; set; }
    }
}
