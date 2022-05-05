using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Migrations;

namespace MOE.Common.Models
{
    public class SPM : IdentityDbContext<SPMUser>
    {
        public SPM()
            :base("SPM")
            //: base("name=SPM")
        {
            

            Database.SetInitializer<SPM>(new CreateDatabaseIfNotExists<SPM>());
            Database.CommandTimeout = 900;
        }

        public SPM(string ConnectionString) : base(ConnectionString)
        {
            Database.SetInitializer(new CustomInitializer());
            Database.Initialize(true);
        }


        public DbSet<SPMRole> IdentityRoles { get; set; }
        public virtual DbSet<ApplicationEvent> ApplicationEvents { get; set; }
        public virtual DbSet<SPMWatchDogErrorEvent> SPMWatchDogErrorEvents { get; set; }
        public virtual DbSet<MetricComment> MetricComments { get; set; }
        public virtual DbSet<DetectorComment> DetectorComments { get; set; }
        public virtual DbSet<MovementType> MovementTypes { get; set; }
        public virtual DbSet<DirectionType> DirectionTypes { get; set; }
        public virtual DbSet<LaneType> LaneTypes { get; set; }
        public virtual DbSet<Approach> Approaches { get; set; }
        public virtual DbSet<DetectionType> DetectionTypes { get; set; }
        public virtual DbSet<MetricsFilterType> MetricsFilterTypes { get; set; }
        public virtual DbSet<MetricType> MetricTypes { get; set; }
        public virtual DbSet<Controller_Event_Log> Controller_Event_Log { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<Detector> Detectors { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Signal> Signals { get; set; }
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<ActionLog> ActionLogs { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RouteSignal> RouteSignals { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Jurisdiction> Jurisdictions { get; set; }
        public virtual DbSet<RoutePhaseDirection> RoutePhaseDirections { get; set; }
        public virtual DbSet<ControllerType> ControllerType { get; set; }
        public virtual DbSet<Speed_Events> Speed_Events { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<ExternalLink> ExternalLinks { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public virtual DbSet<WatchDogApplicationSettings> WatchdogApplicationSettings { get; set; }
        public virtual DbSet<DatabaseArchiveSettings> DatabaseArchiveSettings { get; set; }
        public virtual DbSet<DatabaseArchiveExcludedSignal> DatabaseArchiveExcludedSignals { get; set; }
        public virtual DbSet<GeneralSettings> GeneralSettings { get; set; }
        public virtual DbSet<DetectionHardware> DetectionHardwares { get; set; }
        public virtual DbSet<VersionAction> VersionActions { get; set; }
        public virtual DbSet<PreemptionAggregation> PreemptionAggregations { get; set; }
        public virtual DbSet<PriorityAggregation> PriorityAggregations { get; set; }
        public virtual DbSet<PhaseCycleAggregation> PhaseCycleAggregations { get; set; }
        public virtual DbSet<ApproachPcdAggregation> ApproachPcdAggregations { get; set; }
        public virtual DbSet<ApproachSplitFailAggregation> ApproachSplitFailAggregations { get; set; }
        public virtual DbSet<PhaseTerminationAggregation> PhaseTerminationAggregations { get; set; }
        public virtual DbSet<PhasePedAggregation> PhasePedAggregations { get; set; }
        public virtual DbSet<SignalEventCountAggregation> SignalEventCountAggregations { get; set; }
       public virtual DbSet<DetectorEventCountAggregation> DetectorEventCountAggregations { get; set; }

        public virtual DbSet<TablePartitionProcessed> TablePartitionProcesseds { get; set; }
        public virtual DbSet<StatusOfProcessedTable> StatusOfProcessedTables { get; set; }
        public virtual DbSet<ToBeProcessededTable> ToBeProcessededTables { get; set; }
        public virtual DbSet<ToBeProcessedTableIndex> ToBeProcessededIndexes { get; set; }

        public virtual DbSet<ApproachYellowRedActivationAggregation> ApproachYellowRedActivationAggregations
        {
            get;
            set;
        }

        public virtual DbSet<ApproachSpeedAggregation> ApproachSpeedAggregations { get; set; }
        public virtual DbSet<SignalPlanAggregation> SignalPlanAggregations { get; set; }
        public virtual DbSet<PhaseSplitMonitorAggregation> PhaseSplitMonitorAggregationsAggregations { get; set; }
        public virtual DbSet<PhaseLeftTurnGapAggregation> PhaseLeftTurnGapAggregations { get; set; }
        public virtual DbSet<SignalToAggregate> SignalsToAggregate { get; set; }
        public virtual DbSet<MeasuresDefaults> MeasuresDefaults { get; set; }

        public static SPM Create()
        {
            return new SPM();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Signal>()
                .Property(e => e.PrimaryName)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.SecondaryName)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.RegionID);

            modelBuilder.Entity<Signal>()
                .HasRequired(s => s.Jurisdiction)
                .WithMany(s => s.Signals)
                .HasForeignKey(u => u.JurisdictionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Approach>()
                .HasMany(e => e.Detectors)
                .WithRequired(e => e.Approach)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Detector>()
                .HasMany(e => e.DetectorComments)
                .WithRequired(e => e.Detector)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Detector>()
                .Property(e => e.DetectorID)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_DetectorIDUnique") {IsUnique = true}));


            modelBuilder.Entity<ControllerType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<ControllerType>()
                .Property(e => e.FTPDirectory)
                .IsUnicode(false);

            modelBuilder.Entity<ControllerType>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<ControllerType>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .HasRequired(s => s.ControllerType)
                .WithMany()
                .HasForeignKey(u => u.ControllerTypeID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Detector>()
                .HasMany(g => g.DetectionTypes)
                .WithMany(d => d.Detectors)
                .Map(mc =>
                    {
                        mc.ToTable("DetectionTypeDetector");
                        mc.MapLeftKey("ID");
                        mc.MapRightKey("DetectionTypeID");
                    }
                );

            modelBuilder.Entity<ActionLog>()
                .HasMany(al => al.Actions);
            modelBuilder.Entity<ActionLog>()
                .HasMany(al => al.MetricTypes);
        }
    }

    public class CustomInitializer : IDatabaseInitializer<SPM>
    {
        #region IDatabaseInitializer<SPM> Members

        // fix the problem with MigrateDatabaseToLatestVersion 
        // by copying the connection string FROM the context
        public void InitializeDatabase(SPM context)
        {
            
            Configuration cfg = new Configuration(); // migration configuration class
            cfg.TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, "System.Data.SqlClient");

          
            DbMigrator dbMigrator = new DbMigrator(cfg);
            if(dbMigrator.GetPendingMigrations().Any())
                // this will call the parameterless constructor of the datacontext
                // but the connection string from above will be then set on in
                dbMigrator.Update();
        }

        #endregion
    }

}