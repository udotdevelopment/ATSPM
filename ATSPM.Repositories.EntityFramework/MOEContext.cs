using System;
using ATSPM.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public partial class MOEContext : DbContext
    {
        public MOEContext()
        {
        }

        public MOEContext(DbContextOptions<MOEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application.Models.Action> Actions { get; set; }
        public virtual DbSet<ActionLog> ActionLogs { get; set; }
        public virtual DbSet<ActionLogAction> ActionLogActions { get; set; }
        public virtual DbSet<ActionLogMetricType> ActionLogMetricTypes { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<Application.Models.Application> Applications { get; set; }
        public virtual DbSet<ApplicationEvent> ApplicationEvents { get; set; }
        public virtual DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public virtual DbSet<Approach> Approaches { get; set; }
        public virtual DbSet<ApproachPcdAggregation> ApproachPcdAggregations { get; set; }
        public virtual DbSet<ApproachSpeedAggregation> ApproachSpeedAggregations { get; set; }
        public virtual DbSet<ApproachSplitFailAggregation> ApproachSplitFailAggregations { get; set; }
        public virtual DbSet<ApproachYellowRedActivationAggregation> ApproachYellowRedActivationAggregations { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ControllerEventLog> ControllerEventLogs { get; set; }
        public virtual DbSet<ControllerType> ControllerTypes { get; set; }
        public virtual DbSet<DatabaseArchiveExcludedSignal> DatabaseArchiveExcludedSignals { get; set; }
        public virtual DbSet<DetectionHardware> DetectionHardwares { get; set; }
        public virtual DbSet<DetectionType> DetectionTypes { get; set; }
        public virtual DbSet<DetectionTypeDetector> DetectionTypeDetectors { get; set; }
        public virtual DbSet<DetectionTypeMetricType> DetectionTypeMetricTypes { get; set; }
        public virtual DbSet<Detector> Detectors { get; set; }
        public virtual DbSet<DetectorComment> DetectorComments { get; set; }
        public virtual DbSet<DetectorEventCountAggregation> DetectorEventCountAggregations { get; set; }
        public virtual DbSet<DirectionType> DirectionTypes { get; set; }
        public virtual DbSet<ExternalLink> ExternalLinks { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<LaneType> LaneTypes { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MetricComment> MetricComments { get; set; }
        public virtual DbSet<MetricCommentMetricType> MetricCommentMetricTypes { get; set; }
        public virtual DbSet<MetricType> MetricTypes { get; set; }
        public virtual DbSet<MetricsFilterType> MetricsFilterTypes { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }
        public virtual DbSet<MovementType> MovementTypes { get; set; }
        public virtual DbSet<PhaseCycleAggregation> PhaseCycleAggregations { get; set; }
        public virtual DbSet<PhaseLeftTurnGapAggregation> PhaseLeftTurnGapAggregations { get; set; }
        public virtual DbSet<PhasePedAggregation> PhasePedAggregations { get; set; }
        public virtual DbSet<PhaseSplitMonitorAggregation> PhaseSplitMonitorAggregations { get; set; }
        public virtual DbSet<PhaseTerminationAggregation> PhaseTerminationAggregations { get; set; }
        public virtual DbSet<PreemptionAggregation> PreemptionAggregations { get; set; }
        public virtual DbSet<PriorityAggregation> PriorityAggregations { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RoutePhaseDirection> RoutePhaseDirections { get; set; }
        public virtual DbSet<RouteSignal> RouteSignals { get; set; }
        public virtual DbSet<Signal> Signals { get; set; }
        public virtual DbSet<SignalEventCountAggregation> SignalEventCountAggregations { get; set; }
        public virtual DbSet<SignalPlanAggregation> SignalPlanAggregations { get; set; }
        public virtual DbSet<SignalToAggregate> SignalToAggregates { get; set; }
        public virtual DbSet<SpeedEvent> SpeedEvents { get; set; }
        public virtual DbSet<SpmwatchDogErrorEvent> SpmwatchDogErrorEvents { get; set; }
        public virtual DbSet<StatusOfProcessedTable> StatusOfProcessedTables { get; set; }
        public virtual DbSet<TablePartitionProcessed> TablePartitionProcesseds { get; set; }
        public virtual DbSet<ToBeProcessedTableIndex> ToBeProcessedTableIndexes { get; set; }
        public virtual DbSet<ToBeProcessededIndex> ToBeProcessededIndexes { get; set; }
        public virtual DbSet<ToBeProcessededTable> ToBeProcessededTables { get; set; }
        public virtual DbSet<VersionAction> VersionActions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=MOE;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Application.Models.Action>(entity =>
            {
                entity.Property(e => e.ActionId).HasColumnName("ActionID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ActionLog>(entity =>
            {
                entity.HasIndex(e => e.AgencyId, "IX_AgencyID");

                entity.Property(e => e.ActionLogId).HasColumnName("ActionLogID");

                entity.Property(e => e.AgencyId).HasColumnName("AgencyID");

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SignalId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SignalID");

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.ActionLogs)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("FK_dbo.ActionLogs_dbo.Agencies_AgencyID");
            });

            modelBuilder.Entity<ActionLogAction>(entity =>
            {
                entity.HasKey(e => new { e.ActionLogActionLogId, e.ActionActionId })
                    .HasName("PK_dbo.ActionLogActions");

                entity.HasIndex(e => e.ActionLogActionLogId, "IX_ActionLog_ActionLogID");

                entity.HasIndex(e => e.ActionActionId, "IX_Action_ActionID");

                entity.Property(e => e.ActionLogActionLogId).HasColumnName("ActionLog_ActionLogID");

                entity.Property(e => e.ActionActionId).HasColumnName("Action_ActionID");

                entity.HasOne(d => d.ActionAction)
                    .WithMany(p => p.ActionLogActions)
                    .HasForeignKey(d => d.ActionActionId)
                    .HasConstraintName("FK_dbo.ActionLogActions_dbo.Actions_Action_ActionID");

                entity.HasOne(d => d.ActionLogActionLog)
                    .WithMany(p => p.ActionLogActions)
                    .HasForeignKey(d => d.ActionLogActionLogId)
                    .HasConstraintName("FK_dbo.ActionLogActions_dbo.ActionLogs_ActionLog_ActionLogID");
            });

            modelBuilder.Entity<ActionLogMetricType>(entity =>
            {
                entity.HasKey(e => new { e.ActionLogActionLogId, e.MetricTypeMetricId })
                    .HasName("PK_dbo.ActionLogMetricTypes");

                entity.HasIndex(e => e.ActionLogActionLogId, "IX_ActionLog_ActionLogID");

                entity.HasIndex(e => e.MetricTypeMetricId, "IX_MetricType_MetricID");

                entity.Property(e => e.ActionLogActionLogId).HasColumnName("ActionLog_ActionLogID");

                entity.Property(e => e.MetricTypeMetricId).HasColumnName("MetricType_MetricID");

                entity.HasOne(d => d.ActionLogActionLog)
                    .WithMany(p => p.ActionLogMetricTypes)
                    .HasForeignKey(d => d.ActionLogActionLogId)
                    .HasConstraintName("FK_dbo.ActionLogMetricTypes_dbo.ActionLogs_ActionLog_ActionLogID");

                entity.HasOne(d => d.MetricTypeMetric)
                    .WithMany(p => p.ActionLogMetricTypes)
                    .HasForeignKey(d => d.MetricTypeMetricId)
                    .HasConstraintName("FK_dbo.ActionLogMetricTypes_dbo.MetricTypes_MetricType_MetricID");
            });

            modelBuilder.Entity<Agency>(entity =>
            {
                entity.Property(e => e.AgencyId).HasColumnName("AgencyID");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Application.Models.Application>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<ApplicationEvent>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationName).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<ApplicationSetting>(entity =>
            {
                entity.HasIndex(e => e.ApplicationId, "IX_ApplicationID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.Discriminator)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PreviousDayPmpeakEnd).HasColumnName("PreviousDayPMPeakEnd");

                entity.Property(e => e.PreviousDayPmpeakStart).HasColumnName("PreviousDayPMPeakStart");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationSettings)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_dbo.ApplicationSettings_dbo.Applications_ApplicationID");
            });

            modelBuilder.Entity<Approach>(entity =>
            {
                entity.HasIndex(e => e.DirectionTypeId, "IX_DirectionTypeID");

                entity.HasIndex(e => e.VersionId, "IX_VersionID");

                entity.Property(e => e.ApproachId).HasColumnName("ApproachID");

                entity.Property(e => e.DirectionTypeId).HasColumnName("DirectionTypeID");

                entity.Property(e => e.Mph).HasColumnName("MPH");

                entity.Property(e => e.SignalId).HasColumnName("SignalID");

                entity.Property(e => e.VersionId).HasColumnName("VersionID");

                entity.HasOne(d => d.DirectionType)
                    .WithMany(p => p.Approaches)
                    .HasForeignKey(d => d.DirectionTypeId)
                    .HasConstraintName("FK_dbo.Approaches_dbo.DirectionTypes_DirectionTypeID");

                entity.HasOne(d => d.Signal)
                    .WithMany(p => p.Approaches)
                    .HasForeignKey(d => d.VersionId)
                    .HasConstraintName("FK_dbo.Approaches_dbo.Signals_VersionId");
            });

            modelBuilder.Entity<ApproachPcdAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PhaseNumber, e.IsProtectedPhase })
                    .HasName("PK_dbo.ApproachPcdAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<ApproachSpeedAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.ApproachId })
                    .HasName("PK_dbo.ApproachSpeedAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<ApproachSplitFailAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.ApproachId, e.PhaseNumber, e.IsProtectedPhase })
                    .HasName("PK_dbo.ApproachSplitFailAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<ApproachYellowRedActivationAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PhaseNumber, e.IsProtectedPhase })
                    .HasName("PK_dbo.ApproachYellowRedActivationAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.Name, "RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Discriminator)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.UserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.HasIndex(e => e.UserId, "IX_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_dbo.AspNetUserRoles");

                entity.HasIndex(e => e.RoleId, "IX_RoleId");

                entity.HasIndex(e => e.UserId, "IX_UserId");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.Comment1)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Comment");

                entity.Property(e => e.Entity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<ControllerEventLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ControllerEventLogs");

                entity.HasIndex(e => e.Timestamp, "IX_Clustered_Controller_Event_Log_Timestamp")
                    .IsClustered();

                entity.HasIndex(e => new { e.SignalId, e.Timestamp, e.EventCode, e.EventParam }, "IX_SignalID_Timestamp_EventCode_EventParam");

                entity.Property(e => e.SignalId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SignalID");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<ControllerType>(entity =>
            {
                entity.Property(e => e.ControllerTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ControllerTypeID");

                entity.Property(e => e.ActiveFtp).HasColumnName("ActiveFTP");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ftpdirectory)
                    .IsUnicode(false)
                    .HasColumnName("FTPDirectory");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Snmpport).HasColumnName("SNMPPort");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DatabaseArchiveExcludedSignal>(entity =>
            {
                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<DetectionHardware>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<DetectionType>(entity =>
            {
                entity.Property(e => e.DetectionTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("DetectionTypeID");

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<DetectionTypeDetector>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.DetectionTypeId })
                    .HasName("PK_dbo.DetectionTypeDetector");

                entity.ToTable("DetectionTypeDetector");

                entity.HasIndex(e => e.DetectionTypeId, "IX_DetectionTypeID");

                entity.HasIndex(e => e.Id, "IX_ID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DetectionTypeId).HasColumnName("DetectionTypeID");

                entity.HasOne(d => d.DetectionType)
                    .WithMany(p => p.DetectionTypeDetectors)
                    .HasForeignKey(d => d.DetectionTypeId)
                    .HasConstraintName("FK_dbo.DetectionTypeDetector_dbo.DetectionTypes_DetectionTypeID");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.DetectionTypeDetectors)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_dbo.DetectionTypeDetector_dbo.Detectors_ID");
            });

            modelBuilder.Entity<DetectionTypeMetricType>(entity =>
            {
                entity.HasKey(e => new { e.DetectionTypeDetectionTypeId, e.MetricTypeMetricId })
                    .HasName("PK_dbo.DetectionTypeMetricTypes");

                entity.HasIndex(e => e.DetectionTypeDetectionTypeId, "IX_DetectionType_DetectionTypeID");

                entity.HasIndex(e => e.MetricTypeMetricId, "IX_MetricType_MetricID");

                entity.Property(e => e.DetectionTypeDetectionTypeId).HasColumnName("DetectionType_DetectionTypeID");

                entity.Property(e => e.MetricTypeMetricId).HasColumnName("MetricType_MetricID");

                entity.HasOne(d => d.DetectionTypeDetectionType)
                    .WithMany(p => p.DetectionTypeMetricTypes)
                    .HasForeignKey(d => d.DetectionTypeDetectionTypeId)
                    .HasConstraintName("FK_dbo.DetectionTypeMetricTypes_dbo.DetectionTypes_DetectionType_DetectionTypeID");

                entity.HasOne(d => d.MetricTypeMetric)
                    .WithMany(p => p.DetectionTypeMetricTypes)
                    .HasForeignKey(d => d.MetricTypeMetricId)
                    .HasConstraintName("FK_dbo.DetectionTypeMetricTypes_dbo.MetricTypes_MetricType_MetricID");
            });

            modelBuilder.Entity<Detector>(entity =>
            {
                entity.HasIndex(e => e.ApproachId, "IX_ApproachID");

                entity.HasIndex(e => e.DetectionHardwareId, "IX_DetectionHardwareID");

                entity.HasIndex(e => e.LaneTypeId, "IX_LaneTypeID");

                entity.HasIndex(e => e.MovementTypeId, "IX_MovementTypeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproachId).HasColumnName("ApproachID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateDisabled).HasColumnType("datetime");

                entity.Property(e => e.DetectionHardwareId).HasColumnName("DetectionHardwareID");

                entity.Property(e => e.DetectorId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("DetectorID");

                entity.Property(e => e.LaneTypeId).HasColumnName("LaneTypeID");

                entity.Property(e => e.MovementTypeId).HasColumnName("MovementTypeID");

                entity.HasOne(d => d.Approach)
                    .WithMany(p => p.Detectors)
                    .HasForeignKey(d => d.ApproachId)
                    .HasConstraintName("FK_dbo.Detectors_dbo.Approaches_ApproachID");

                entity.HasOne(d => d.DetectionHardware)
                    .WithMany(p => p.Detectors)
                    .HasForeignKey(d => d.DetectionHardwareId)
                    .HasConstraintName("FK_dbo.Detectors_dbo.DetectionHardwares_DetectionHardwareID");

                entity.HasOne(d => d.LaneType)
                    .WithMany(p => p.Detectors)
                    .HasForeignKey(d => d.LaneTypeId)
                    .HasConstraintName("FK_dbo.Detectors_dbo.LaneTypes_LaneTypeID");

                entity.HasOne(d => d.MovementType)
                    .WithMany(p => p.Detectors)
                    .HasForeignKey(d => d.MovementTypeId)
                    .HasConstraintName("FK_dbo.Detectors_dbo.MovementTypes_MovementTypeID");
            });

            modelBuilder.Entity<DetectorComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK_dbo.DetectorComments");

                entity.HasIndex(e => e.Id, "IX_ID");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CommentText).IsRequired();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.DetectorComments)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_dbo.DetectorComments_dbo.Detectors_ID");
            });

            modelBuilder.Entity<DetectorEventCountAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.DetectorPrimaryId })
                    .HasName("PK_dbo.DetectorEventCountAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<DirectionType>(entity =>
            {
                entity.Property(e => e.DirectionTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("DirectionTypeID");

                entity.Property(e => e.Abbreviation).HasMaxLength(5);

                entity.Property(e => e.Description).HasMaxLength(30);
            });

            modelBuilder.Entity<ExternalLink>(entity =>
            {
                entity.Property(e => e.ExternalLinkId).HasColumnName("ExternalLinkID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Url).IsRequired();
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQs");

                entity.Property(e => e.Faqid).HasColumnName("FAQID");

                entity.Property(e => e.Body).IsRequired();

                entity.Property(e => e.Header).IsRequired();
            });

            modelBuilder.Entity<LaneType>(entity =>
            {
                entity.Property(e => e.LaneTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("LaneTypeID");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuId).ValueGeneratedNever();

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Controller)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MetricComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK_dbo.MetricComments");

                entity.HasIndex(e => e.VersionId, "IX_VersionID");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CommentText).IsRequired();

                entity.Property(e => e.SignalId)
                    .HasMaxLength(10)
                    .HasColumnName("SignalID");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.VersionId).HasColumnName("VersionID");
            });

            modelBuilder.Entity<MetricCommentMetricType>(entity =>
            {
                entity.HasKey(e => new { e.MetricCommentCommentId, e.MetricTypeMetricId })
                    .HasName("PK_dbo.MetricCommentMetricTypes");

                entity.HasIndex(e => e.MetricCommentCommentId, "IX_MetricComment_CommentID");

                entity.HasIndex(e => e.MetricTypeMetricId, "IX_MetricType_MetricID");

                entity.Property(e => e.MetricCommentCommentId).HasColumnName("MetricComment_CommentID");

                entity.Property(e => e.MetricTypeMetricId).HasColumnName("MetricType_MetricID");

                entity.HasOne(d => d.MetricCommentComment)
                    .WithMany(p => p.MetricCommentMetricTypes)
                    .HasForeignKey(d => d.MetricCommentCommentId)
                    .HasConstraintName("FK_dbo.MetricCommentMetricTypes_dbo.MetricComments_MetricComment_CommentID");

                entity.HasOne(d => d.MetricTypeMetric)
                    .WithMany(p => p.MetricCommentMetricTypes)
                    .HasForeignKey(d => d.MetricTypeMetricId)
                    .HasConstraintName("FK_dbo.MetricCommentMetricTypes_dbo.MetricTypes_MetricType_MetricID");
            });

            modelBuilder.Entity<MetricType>(entity =>
            {
                entity.HasKey(e => e.MetricId)
                    .HasName("PK_dbo.MetricTypes");

                entity.Property(e => e.MetricId)
                    .ValueGeneratedNever()
                    .HasColumnName("MetricID");

                entity.Property(e => e.Abbreviation).IsRequired();

                entity.Property(e => e.ChartName).IsRequired();
            });

            modelBuilder.Entity<MetricsFilterType>(entity =>
            {
                entity.HasKey(e => e.FilterId)
                    .HasName("PK_dbo.MetricsFilterTypes");

                entity.Property(e => e.FilterId).HasColumnName("FilterID");

                entity.Property(e => e.FilterName).IsRequired();
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<MovementType>(entity =>
            {
                entity.Property(e => e.MovementTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("MovementTypeID");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<PhaseCycleAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PhaseNumber })
                    .HasName("PK_dbo.PhaseCycleAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<PhaseLeftTurnGapAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PhaseNumber })
                    .HasName("PK_dbo.PhaseLeftTurnGapAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<PhasePedAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PhaseNumber })
                    .HasName("PK_dbo.PhasePedAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<PhaseSplitMonitorAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PhaseNumber })
                    .HasName("PK_dbo.PhaseSplitMonitorAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(128);
            });

            modelBuilder.Entity<PhaseTerminationAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PhaseNumber })
                    .HasName("PK_dbo.PhaseTerminationAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<PreemptionAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PreemptNumber })
                    .HasName("PK_dbo.PreemptionAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<PriorityAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId, e.PriorityNumber })
                    .HasName("PK_dbo.PriorityAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.Property(e => e.RouteName).IsRequired();
            });

            modelBuilder.Entity<RoutePhaseDirection>(entity =>
            {
                entity.HasIndex(e => e.DirectionTypeId, "IX_DirectionTypeId");

                entity.HasIndex(e => e.RouteSignalId, "IX_RouteSignalId");

                entity.HasOne(d => d.DirectionType)
                    .WithMany(p => p.RoutePhaseDirections)
                    .HasForeignKey(d => d.DirectionTypeId)
                    .HasConstraintName("FK_dbo.RoutePhaseDirections_dbo.DirectionTypes_DirectionTypeId");

                entity.HasOne(d => d.RouteSignal)
                    .WithMany(p => p.RoutePhaseDirections)
                    .HasForeignKey(d => d.RouteSignalId)
                    .HasConstraintName("FK_dbo.RoutePhaseDirections_dbo.RouteSignals_RouteSignalId");
            });

            modelBuilder.Entity<RouteSignal>(entity =>
            {
                entity.HasIndex(e => e.RouteId, "IX_RouteId");

                entity.Property(e => e.SignalId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteSignals)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("FK_dbo.RouteSignals_dbo.Routes_RouteId");
            });

            modelBuilder.Entity<Signal>(entity =>
            {
                entity.HasKey(e => e.VersionId)
                    .HasName("PK_dbo.Signals");

                entity.HasIndex(e => e.ControllerTypeId, "IX_ControllerTypeID");

                entity.HasIndex(e => e.RegionId, "IX_RegionID");

                entity.HasIndex(e => e.VersionActionId, "IX_VersionActionId");

                entity.Property(e => e.VersionId).HasColumnName("VersionID");

                entity.Property(e => e.ControllerTypeId).HasColumnName("ControllerTypeID");

                entity.Property(e => e.IPAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IPAddress")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasDefaultValueSql("('Initial')");

                entity.Property(e => e.PrimaryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.SecondaryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SignalId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SignalID");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.VersionActionId).HasDefaultValueSql("((10))");

                entity.HasOne(d => d.ControllerType)
                    .WithMany(p => p.Signals)
                    .HasForeignKey(d => d.ControllerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Signals_dbo.ControllerTypes_ControllerTypeID");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Signals)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_dbo.Signals_dbo.Region_RegionID");
            });

            modelBuilder.Entity<SignalEventCountAggregation>(entity =>
            {
                entity.HasKey(e => new { e.BinStartTime, e.SignalId })
                    .HasName("PK_dbo.SignalEventCountAggregations");

                entity.Property(e => e.BinStartTime).HasColumnType("datetime");

                entity.Property(e => e.SignalId).HasMaxLength(10);
            });

            modelBuilder.Entity<SignalPlanAggregation>(entity =>
            {
                entity.HasKey(e => new { e.SignalId, e.Start, e.End })
                    .HasName("PK_dbo.SignalPlanAggregations");

                entity.Property(e => e.SignalId).HasMaxLength(128);

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.End).HasColumnType("datetime");
            });

            modelBuilder.Entity<SignalToAggregate>(entity =>
            {
                entity.HasKey(e => e.SignalId)
                    .HasName("PK_dbo.SignalToAggregates");

                entity.Property(e => e.SignalId)
                    .HasMaxLength(10)
                    .HasColumnName("SignalID");
            });

            modelBuilder.Entity<SpeedEvent>(entity =>
            {
                entity.HasKey(e => new { e.DetectorId, e.Mph, e.Kph, e.Timestamp })
                    .HasName("PK_dbo.Speed_Events");

                entity.ToTable("Speed_Events");

                entity.Property(e => e.DetectorId)
                    .HasMaxLength(50)
                    .HasColumnName("DetectorID");

                entity.Property(e => e.Mph).HasColumnName("MPH");

                entity.Property(e => e.Kph).HasColumnName("KPH");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            });

            modelBuilder.Entity<SpmwatchDogErrorEvent>(entity =>
            {
                entity.ToTable("SPMWatchDogErrorEvents");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DetectorId).HasColumnName("DetectorID");

                entity.Property(e => e.Direction).IsRequired();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.SignalId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SignalID");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<StatusOfProcessedTable>(entity =>
            {
                entity.Property(e => e.SqlstatementOrMessage).HasColumnName("SQLStatementOrMessage");

                entity.Property(e => e.TimeEntered).HasColumnType("datetime");
            });

            modelBuilder.Entity<TablePartitionProcessed>(entity =>
            {
                entity.Property(e => e.FileGroupName).IsRequired();

                entity.Property(e => e.SwapTableName).IsRequired();

                entity.Property(e => e.TimeIndexdropped).HasColumnType("datetime");

                entity.Property(e => e.TimeSwappedTableDropped).HasColumnType("datetime");
            });

            modelBuilder.Entity<ToBeProcessededIndex>(entity =>
            {
                entity.Property(e => e.ClusterText).IsRequired();

                entity.Property(e => e.IndexName).IsRequired();

                entity.Property(e => e.TextForIndex).IsRequired();
            });

            modelBuilder.Entity<ToBeProcessededTable>(entity =>
            {
                entity.Property(e => e.CreateColumns4Table).IsRequired();

                entity.Property(e => e.DataBaseName).IsRequired();

                entity.Property(e => e.InsertValues).IsRequired();

                entity.Property(e => e.PartitionedTableName).IsRequired();

                entity.Property(e => e.PreserveDataSelect).IsRequired();

                entity.Property(e => e.PreserveDataWhere).IsRequired();

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<VersionAction>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
