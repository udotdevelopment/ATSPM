namespace MOE.Common.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SPMCopy : DbContext
    {
        public SPMCopy()
            : base("name=SPM")
        {
        }

        public virtual DbSet<Action_Log> Action_Log { get; set; }
        public virtual DbSet<Action_Log_Action_List> Action_Log_Action_List { get; set; }
        public virtual DbSet<Action_Log_Agency_List> Action_Log_Agency_List { get; set; }
        public virtual DbSet<Action_Log_Metric_List> Action_Log_Metric_List { get; set; }
        public virtual DbSet<Alert_Recipients> Alert_Recipients { get; set; }
        public virtual DbSet<ApproachRoute> ApproachRoutes { get; set; }
        public virtual DbSet<DownloadAgreement> DownloadAgreements { get; set; }
        public virtual DbSet<MetricsFilterType> FilterTypes { get; set; }
        public virtual DbSet<Graph_Detectors> Graph_Detectors { get; set; }
        public virtual DbSet<Lastupdate> Lastupdates { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MetricType> MetricTypes { get; set; }
        public virtual DbSet<Program_Message> Program_Message { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Signal> Signals { get; set; }
        public virtual DbSet<SPM_Comment> SPM_Comment { get; set; }
        public virtual DbSet<SPM_Error> SPM_Error { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Action_Log_Actions> Action_Log_Actions { get; set; }
        public virtual DbSet<Action_Log_Metrics> Action_Log_Metrics { get; set; }
        public virtual DbSet<Alert_Types> Alert_Types { get; set; }
        public virtual DbSet<ApproachDirection> ApproachDirections { get; set; }
        public virtual DbSet<ApproachRouteDetail> ApproachRouteDetails { get; set; }
        public virtual DbSet<Archived_Metrics> Archived_Metrics { get; set; }
        public virtual DbSet<Archived_Metrics_Temp> Archived_Metrics_Temp { get; set; }
        public virtual DbSet<Controller_Type> Controller_Type { get; set; }
        public virtual DbSet<Detector_Comment> Detector_Comment { get; set; }
        public virtual DbSet<Detector_Error> Detector_Error { get; set; }
        public virtual DbSet<MOE_Users> MOE_Users { get; set; }
        public virtual DbSet<Program_Settings> Program_Settings { get; set; }
        public virtual DbSet<Route_Detectors> Route_Detectors { get; set; }
        public virtual DbSet<Speed_Events> Speed_Events { get; set; }
        public virtual DbSet<Controller_Event_Log> Controller_Event_Log { get; set; }
        public virtual DbSet<Accordian> Accordians { get; set; }
        public virtual DbSet<Alert_Day_Types> Alert_Day_Types { get; set; }
        public virtual DbSet<Error_Types> Error_Types { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action_Log>()
                .HasMany(e => e.Action_Log_Actions)
                .WithRequired(e => e.Action_Log)
                .HasForeignKey(e => e.Action_Log_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Action_Log>()
                .HasMany(e => e.Action_Log_Metrics)
                .WithRequired(e => e.Action_Log)
                .HasForeignKey(e => e.Action_Log_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Action_Log_Action_List>()
                .HasMany(e => e.Action_Log_Actions)
                .WithRequired(e => e.Action_Log_Action_List)
                .HasForeignKey(e => e.Action_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Action_Log_Agency_List>()
                .HasMany(e => e.Action_Log)
                .WithOptional(e => e.Action_Log_Agency_List)
                .HasForeignKey(e => e.Agency);

            modelBuilder.Entity<Action_Log_Metric_List>()
                .HasMany(e => e.Action_Log_Metrics)
                .WithRequired(e => e.Action_Log_Metric_List)
                .HasForeignKey(e => e.Metric_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApproachRoute>()
                .Property(e => e.RouteName)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.Lane)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.Phase)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.Loops)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.Direction)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.IPaddr)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.TMC_Lane_Type)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .Property(e => e.Perm_Phase)
                .IsUnicode(false);

            modelBuilder.Entity<Graph_Detectors>()
                .HasMany(e => e.Detector_Comment)
                .WithRequired(e => e.Graph_Detectors)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Graph_Detectors>()
                .HasMany(e => e.Detector_Error)
                .WithRequired(e => e.Graph_Detectors)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Graph_Detectors>()
                .HasMany(e => e.Route_Detectors)
                .WithRequired(e => e.Graph_Detectors)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Region>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.Primary_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.Secondary_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.IP_Address)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .Property(e => e.Region)
                .IsUnicode(false);

            modelBuilder.Entity<Signal>()
                .HasMany(e => e.Action_Log)
                .WithRequired(e => e.Signal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Signal>()
                .HasMany(e => e.Graph_Detectors)
                .WithRequired(e => e.Signal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Signal>()
                .HasOptional(e => e.Lastupdate)
                .WithRequired(e => e.Signal);

            modelBuilder.Entity<Signal>()
                .HasMany(e => e.ApproachRouteDetails)
                .WithRequired(e => e.Signal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SPM_Comment>()
                .Property(e => e.Entity)
                .IsUnicode(false);

            modelBuilder.Entity<SPM_Comment>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<SPM_Error>()
                .Property(e => e.Param1)
                .IsUnicode(false);

            modelBuilder.Entity<SPM_Error>()
                .Property(e => e.Param2)
                .IsUnicode(false);

            modelBuilder.Entity<ApproachDirection>()
                .Property(e => e.DirectionName)
                .IsUnicode(false);

            modelBuilder.Entity<Archived_Metrics>()
                .Property(e => e.DetectorID)
                .IsUnicode(false);

            modelBuilder.Entity<Controller_Type>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Controller_Type>()
                .Property(e => e.FTPDirectory)
                .IsUnicode(false);

            modelBuilder.Entity<Controller_Type>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Controller_Type>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
