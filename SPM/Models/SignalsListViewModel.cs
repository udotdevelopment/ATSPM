using MOE.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPM.Models
{

    public class SignalsListViewModel
    {
        public List<SignalViewModel> Signals { get; set; }

        public SignalLookupViewModel Lookups { get; set; }

        public int TotalCount { get; set; }
        public int TotalCountInQuery { get; set; }

        public SignalsListViewModel()
        {
            Signals = new List<SignalViewModel>();
            Lookups = new SignalLookupViewModel();
        }
    }

    public class SignalViewModel
    {
        public int VersionID { get; set; }

        public int VersionActionId { get; set; }

        public string Note { get; set; }

        public List<MetricCommentViewModel> Comments { get; set; }

        public string SignalID { get; set; }

        public string Start { get; set; }

        public int RegionID { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string PrimaryName { get; set; }

        public string SecondaryName { get; set; }

        public string IPAddress { get; set; }

        public RegionViewModel Region { get; set; }

        public int ControllerTypeID { get; set; }

        public ControllerTypeViewModel ControllerType { get; set; }
        public bool Enabled { get; set; }

        public List<ApproachViewModel> Approaches { get; set; }

        public List<ChartTypeViewModel> AvailableCharts { get; set; }
    }

    public class ApproachesViewModel
    {
        public List<ApproachViewModel> Approaches { get; set; } = new List<ApproachViewModel>();

        public int TotalCount { get; set; } = 0;

        public string SignalID { get; set; }

        public ApproachesViewModel()
        {
            Approaches = new List<ApproachViewModel>();
        }
    }

    public class DetectorsViewModel
    {
        public List<DetectorViewModel> Detectors { get; set; }

        public int TotalCount { get; set; }

        public DetectorsViewModel()
        {
            Detectors = new List<DetectorViewModel>();
        }
    }

    public class ChartTypeViewModel
    {
        public int MetricID { get; set; }

        public string ChartName { get; set; }

        //public bool IsSupportedForSignal { get; set; }

        //public ChartNotSupportedReason NotSupportedReason { get; set; } = ChartNotSupportedReason.None;
    }

    public class MetricCommentViewModel : Comment
    {
        public int VersionID { get; set; }

        public string SignalID { get; set; }

    }

    public enum ChartNotSupportedReason
    {
        None = 0,
        InsufficientDetection = 1,
        NoDetectorsConfigured = 2,
        NoApproachesConfigured = 3
    }

    public class ApproachViewModel
    {
        public int ApproachID { get; set; }

        public string SignalID { get; set; }

        public string SignalName { get; set; }

        public int DirectionTypeID { get; set; }
        public string Direction { get; set; }

        public DirectionTypeViewModel DirectionType { get; set; }

        public string Description { get; set; }

        public int? MPH { get; set; }

        public int ProtectedPhaseNumber { get; set; }

        public int? PermissivePhaseNumber { get; set; }

        public bool IsProtectedPhaseOverlap { get; set; }

        public List<DetectorViewModel> Detectors { get; set; }

        public ApproachViewModel()
        {
            Detectors = new List<DetectorViewModel>();
        }
    }

    public class DetectorViewModel
    {
        public int ID { get; set; }

        public string DetectorID { get; set; }

        public int DetChannel { get; set; }

        public int? DistanceFromStopBar { get; set; }

        public int? MinSpeedFilter { get; set; }

        public DateTime? DateDisabled { get; set; }

        public int? LaneNumber { get; set; }

        public int LaneTypeID { get; set; }
        public int MovementTypeID { get; set; }
        public MovementTypeViewModel MovementType { get; set; }

        public LaneTypeViewModel LaneType { get; set; }

        public List<DetectionTypeViewModel> DetectionTypes { get; set; }
        public int? DecisionPoint { get; set; }

        public int? MovementDelay { get; set; }

        public int ApproachID { get; set; }
        public int DetectionTypeID { get; set; }
        public string Description { get; set; }

    }

    public class DetectionTypeViewModel
    {
        public DetectionTypeViewModel()
        {
            MetricTypes = new HashSet<MetricTypeViewModel>();
        }

        public int DetectionTypeID { get; set; }

        public string Description { get; set; }

        public virtual ICollection<MetricTypeViewModel> MetricTypes { get; set; }

    }
    public class MetricTypeViewModel
    {
        public int MetricID { get; set; }

        public string ChartName { get; set; }

        public string Abbreviation { get; set; }

        public bool ShowOnWebsite { get; set; }

        public int DetectionTypeID { get; set; }

    }

    public class MovementTypeViewModel
    {
        public int MovementTypeID { get; set; }

        public string Description { get; set; }

        public string Abbreviation { get; set; }
    }

    public class LaneTypeViewModel
    {
        public int LaneTypeID { get; set; }

        public string Description { get; set; }

        public string Abbreviation { get; set; }
    }


    public class DirectionTypeViewModel
    {
        public int DirectionTypeID { get; set; }

        public string Description { get; set; }

        public string Abbreviation { get; set; }
    }

    public class ControllerTypeViewModel
    {
        public int ControllerTypeID { get; set; }

        public string Description { get; set; }
        public long SNMPPort { get; set; }

        public string FTPDirectory { get; set; }

        public bool ActiveFTP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RegionViewModel
    {
        public int ID { get; set; }

        public string Description { get; set; }
    }


    public class SignalLookupViewModel
    {
        public List<LookupTypeViewModel> ControllerTypes { get; set; }
        public List<LookupTypeViewModel> DetectionTypes { get; set; }
        public List<LookupTypeViewModel> DirectionTypes { get; set; }
        public List<LookupTypeViewModel> LaneTypes { get; set; }
        public List<LookupTypeViewModel> MovementTypes { get; set; }


        public SignalLookupViewModel()
        {
            ControllerTypes = new List<LookupTypeViewModel>();
            DetectionTypes = new List<LookupTypeViewModel>();
            LaneTypes = new List<LookupTypeViewModel>();
            MovementTypes = new List<LookupTypeViewModel>();
            DirectionTypes = new List<LookupTypeViewModel>();
        }
    }
    public class LookupTypeViewModel
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string ExtraData { get; set; }

    }


}