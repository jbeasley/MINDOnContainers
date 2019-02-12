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
                .ForMember(dest => dest.RoutingInstanceName, conf => conf.MapFrom(src => src.GetRoutingInstance().Name))
                .ForMember(dest => dest.AdministratorSubField, conf => conf.MapFrom(src => src.GetRoutingInstance().AdministratorSubField))
                .ForMember(dest => dest.AssignedNumberSubField, conf => conf.MapFrom(src => src.GetRoutingInstance().AssignedNumberSubField))
                .ForMember(dest => dest.AttachmentId, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, conf => conf.MapFrom(src => src.GetLocationName()))
                .ForMember(dest => dest.PlaneName, conf => conf.MapFrom(src => src.GetPlaneName()));

            CreateMap<BundleAttachment, AttachmentDTO>()
                .ForMember(dest => dest.RoutingInstanceName, conf => conf.MapFrom(src => src.GetRoutingInstance().Name))
                .ForMember(dest => dest.AdministratorSubField, conf => conf.MapFrom(src => src.GetRoutingInstance().AdministratorSubField))
                .ForMember(dest => dest.AssignedNumberSubField, conf => conf.MapFrom(src => src.GetRoutingInstance().AssignedNumberSubField))
                .ForMember(dest => dest.AttachmentId, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, conf => conf.MapFrom(src => src.GetLocationName()))
                .ForMember(dest => dest.PlaneName, conf => conf.MapFrom(src => src.GetPlaneName()));
        }
    }
}