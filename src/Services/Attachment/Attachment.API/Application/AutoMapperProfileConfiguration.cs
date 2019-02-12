using AutoMapper;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.API.Application.Commands
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            // Map between Entity Models and DTOs

            CreateMap<SingleAttachment, AttachmentDTO>()
                .ForMember(dest => dest.UniName, conf => conf.MapFrom(src => src.Uni.Name))
                .ForMember(dest => dest.RoutingInstanceId, conf => conf.MapFrom(src => src.Uni.RoutingInstanceId))
                .ForMember(dest => dest.AttachmentId, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, conf => conf.MapFrom(src => src.GetLocationName()))
                .ForMember(dest => dest.PlaneName, conf => conf.MapFrom(src => src.GetPlaneName()));

            CreateMap<BundleAttachment, AttachmentDTO>()
                .ForMember(dest => dest.UniName, conf => conf.MapFrom(src => src.Uni.Name))
                .ForMember(dest => dest.RoutingInstanceId, conf => conf.MapFrom(src => src.Uni.RoutingInstanceId))
                .ForMember(dest => dest.AttachmentId, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, conf => conf.MapFrom(src => src.GetLocationName()))
                .ForMember(dest => dest.PlaneName, conf => conf.MapFrom(src => src.GetPlaneName()));
        }
    }
}