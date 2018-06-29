namespace MOE.Common.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Microsoft.AspNet.Identity.EntityFramework;
    using  MOE.Common.Business.SiteSecurity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Infrastructure.Annotations;

    public partial class SPM : IdentityDbContext<MOE.Common.Business.SiteSecurity.SPMUser>
    {
         public SPM()
            : base("name=SPM")
        {
            Database.SetInitializer<SPM>(new CreateDatabaseIfNotExists<SPM>());
            Database.CommandTimeout = 900;
        }

        public static MOE.Common.Models.SPM Create()
        {
            return new MOE.Common.Models.SPM();
        }


       
        public System.Data.Entity.DbSet<MOE.Common.Business.SiteSecurity.SPMRole> IdentityRoles { get; set; }
        
        public virtual DbSet<ApplicationEvent> ApplicationEvents { get; set; }
        public virtual DbSet<SPMWatchDogErrorEvent> SPMWatchDogErrorEvents { get; set; }
        public virtual DbSet<MetricComment> MetricComments { get; set; }
        public virtual DbSet<DetectorComment> DetectorComments { get; set; }
        public virtual DbSet<MovementType> MovementTypes { get; set; }
        public virtual DbSet<DirectionType> DirectionTypes { get; set; }
        public virtual DbSet<LaneType> LaneTypes { get; set; }
        public virtual DbSet<Approach> Approaches { get; set; }
        public virtual DbSet<MOE.Common.Models.Custom.SignalWithDetection> SignalsWithDetection { get; set; }
        public virtual DbSet<DetectionType> DetectionTypes { get; set; }
        public virtual DbSet<MetricsFilterType> MetricsFilterTypes { get; set; }
        public virtual DbSet<MetricType> MetricTypes { get; set; }
        public virtual DbSet<Controller_Event_Log> Controller_Event_Log { get; set; }        
        public virtual DbSet<Accordian> Accordians { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<Alert_Day_Types> Alert_Day_Types { get; set; }
        public virtual DbSet<ApproachRoute> ApproachRoutes { get; set; }
        public virtual DbSet<DownloadAgreement> DownloadAgreements { get; set; }
        public virtual DbSet<Detector> Detectors { get; set; }
        public virtual DbSet<LastUpdate> Lastupdates { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Program_Message> Program_Message { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Signal> Signals { get; set; }
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<ActionLog> ActionLogs { get; set; }
        public virtual DbSet<ApproachRouteDetail> ApproachRouteDetails { get; set; }
        public virtual DbSet<Archived_Metrics> Archived_Metrics { get; set; }
        public virtual DbSet<ControllerType> ControllerType { get; set; }
        public virtual DbSet<Program_Settings> Program_Settings { get; set; }
        public virtual DbSet<Route_Detectors> Route_Detectors { get; set; }
        public virtual DbSet<Speed_Events> Speed_Events { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<ExternalLink> ExternalLinks { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public virtual DbSet<WatchDogApplicationSettings> WatchdogApplicationSettings { get; set; }
        public virtual DbSet<DetectionHardware> DetectionHardwares { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ApproachRoute>()
                .Property(e => e.RouteName)
                .IsUnicode(false);

            modelBuilder.Entity<ApproachRoute>()
                .HasMany(e => e.ApproachRouteDetails)
                .WithRequired(e => e.ApproachRoute)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Approach>()
                .HasMany(e => e.ApproachRouteDetails)
                .WithRequired(e => e.Approach)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Route>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.Name)
                .IsUnicode(false);

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
                .HasMany(e => e.ActionLog)
                .WithRequired(e => e.Signal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Signal>()
                .HasMany(e => e.Approaches)
                .WithRequired(e => e.Signal)
                .WillCascadeOnDelete(true);

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
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_DetectorIDUnique") { IsUnique = true }));


            modelBuilder.Entity<Archived_Metrics>()
                .Property(e => e.DetectorID)
                .IsUnicode(false);

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
}
