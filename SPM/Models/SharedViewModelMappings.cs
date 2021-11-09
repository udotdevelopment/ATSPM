using AutoMapper;
using MOE.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public static class SharedViewModelMappings
    {

        public static void Configure()
        {
            try
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile(new SignalProfile());
                    cfg.AddProfile(new ApproachProfile());
                    cfg.AddProfile(new DetectorProfile());
                });
            }
            //mapper already initialzed
            catch (Exception e) { }
        }

        public static List<Profile> GetProfiles()
        {
            return new List<Profile>()
            {
                new SignalProfile(),
                new ApproachProfile(),
                new LookupDataProfile(),
            };
        }
    }

    public class LookupDataProfile : Profile
    {
        public LookupDataProfile()
        {
            CreateMap<DirectionType, LookupTypeViewModel>()
                 .ForMember(vm => vm.ID, m => m.MapFrom(z => z.DirectionTypeID))
                 .ForMember(vm => vm.Description, m => m.MapFrom(z => z.Description))
                 .ForMember(vm => vm.ExtraData, m => m.MapFrom(z => z.Abbreviation));

            CreateMap<LaneType, LookupTypeViewModel>()
                 .ForMember(vm => vm.ID, m => m.MapFrom(z => z.LaneTypeID))
                 .ForMember(vm => vm.Description, m => m.MapFrom(z => z.Description))
                 .ForMember(vm => vm.ExtraData, m => m.MapFrom(z => z.Abbreviation));

            CreateMap<DetectionType, LookupTypeViewModel>()
                 .ForMember(vm => vm.ID, m => m.MapFrom(z => z.DetectionTypeID))
                 .ForMember(vm => vm.Description, m => m.MapFrom(z => z.Description));

            CreateMap<MovementType, LookupTypeViewModel>()
                 .ForMember(vm => vm.ID, m => m.MapFrom(z => z.MovementTypeID))
                 .ForMember(vm => vm.Description, m => m.MapFrom(z => z.Description))
                 .ForMember(vm => vm.ExtraData, m => m.MapFrom(z => z.Abbreviation));

            CreateMap<ControllerType, LookupTypeViewModel>()
                 .ForMember(vm => vm.ID, m => m.MapFrom(z => z.ControllerTypeID))
                 .ForMember(vm => vm.Description, m => m.MapFrom(z => z.Description))
                 .ForMember(vm => vm.ExtraData, m => m.MapFrom(z => z.SNMPPort));
        }
    }

    public class DetectorProfile : Profile
    {
        public DetectorProfile()
        {
            CreateMap<Detector, DetectorViewModel>();
            CreateMap<DetectorViewModel, Detector>();

            CreateMap<List<Detector>, List<DetectorViewModel>>();
            CreateMap<List<DetectorViewModel>, List<Detector>>();

            CreateMap<MovementType, MovementTypeViewModel>();
            CreateMap<MovementTypeViewModel, MovementType>();

            CreateMap<LaneType, LaneTypeViewModel>();
            CreateMap<LaneTypeViewModel, LaneType>();

            CreateMap<DetectionType, DetectionTypeViewModel>();
            CreateMap<DetectionTypeViewModel, DetectionType>();

            CreateMap<MetricType, MetricTypeViewModel>();
            CreateMap<MetricTypeViewModel, MetricType>();

        }
    }
    public class ApproachProfile : Profile
    {
        public ApproachProfile()
        {

            CreateMap<Approach, ApproachViewModel>()
                .ForMember(vm => vm.Direction, opt => opt.MapFrom(z => z.DirectionType.Description));

            CreateMap<ApproachViewModel, Approach>();
            CreateMap<DirectionType, DirectionTypeViewModel>();
            CreateMap<DirectionTypeViewModel, DirectionType>();
        }
    }

    public class SignalProfile : Profile
    {
        public SignalProfile()
        {
            CreateMap<Signal, SignalViewModel>()
               .ForMember(vm => vm.Approaches, opt => opt.Ignore())
               .ForMember(vm => vm.AvailableCharts, db => db.MapFrom(z => z.GetAvailableMetrics()));

            CreateMap<SignalViewModel, Signal>();

            //SignalPhase objects need to call chart helper to map data
            CreateMap<ControllerType, ControllerTypeViewModel>();
            CreateMap<ControllerTypeViewModel, ControllerType>();
            CreateMap<MetricComment, MetricCommentViewModel>();
        }
    }

}