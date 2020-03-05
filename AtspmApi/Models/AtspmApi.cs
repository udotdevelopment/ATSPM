using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace AtspmApi.Models
{
    public class AtspmApi : DbContext
    {
        public AtspmApi() : base("name=Models.AtspmApi")  
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180; // 180 seconds?
        }
        public virtual DbSet<Approach> Approaches { get; set; }
        public virtual DbSet<ControllerType> ControllerType { get; set; }
        public virtual DbSet<Controller_Event_Log> Controller_Event_Log { get; set; }
        public virtual DbSet<DetectionHardware> DetectionHardwares { get; set; }
        public virtual DbSet<DetectionType> DetectionTypes { get; set; }
        public virtual DbSet<DetectionTypeDetector> DetectionTypeDetectors { get; set; }
        public virtual DbSet<Detector> Detectors { get; set; }
        public virtual DbSet<DirectionType> DirectionTypes { get; set; }
        public virtual DbSet<LaneType> LaneTypes { get; set; }
        public virtual DbSet<MetricType> MetricTypes { get; set; }
        public virtual DbSet<MovementType> MovementTypes { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RouteSignal> RouteSignals { get; set; }
        public virtual DbSet<Signal> Signals { get; set; }
        public virtual DbSet<Speed_Events> Speed_Events { get; set; }
        public virtual DbSet<VersionAction> VersionActions { get; set; }
        public virtual DbSet<PreemptionAggregation> PreemptionAggregations { get; set; }
        public virtual DbSet<PriorityAggregation> PriorityAggregations { get; set; }
        public virtual DbSet<ApproachCycleAggregation> ApproachCycleAggregations { get; set; }
        public virtual DbSet<ApproachPcdAggregation> ApproachPcdAggregations { get; set; }
        public virtual DbSet<ApproachSplitFailAggregation> ApproachSplitFailAggregations { get; set; }
        public virtual DbSet<PhaseTerminationAggregation> PhaseTerminationAggregations { get; set; }
        public virtual DbSet<PhasePedAggregation> PhasePedAggregations { get; set; }
        public virtual DbSet<SignalEventCountAggregation> SignalEventCountAggregations { get; set; }
        public virtual DbSet<ApproachEventCountAggregation> ApproachEventCountAggregations { get; set; }
        public virtual DbSet<DetectorEventCountAggregation> DetectorEventCountAggregations { get; set; }
        public virtual DbSet<ApproachYellowRedActivationAggregation> ApproachYellowRedActivationAggregations
        {
            get;
            set;
        }

        public virtual DbSet<ApproachSpeedAggregation> ApproachSpeedAggregations { get; set; }
        public virtual DbSet<DetectorAggregation> DetectorAggregations { get; set; }

    }
}