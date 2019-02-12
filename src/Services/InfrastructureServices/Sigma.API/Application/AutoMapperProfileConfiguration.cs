using System.Linq;
using AutoMapper;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;
using static MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events.UniCreatedIntegrationEvent;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.Commands
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            // Map between Entity Models and DTOs

            CreateMap<Uni, UniDTO>()
                .ForMember(dest => dest.UniName, conf => conf.MapFrom(src => src.Name))
                .ForMember(dest => dest.RoutingInstanceId, conf => conf.MapFrom(src => src.RoutingInstance.Id))
                .ForMember(dest => dest.UniId, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.UniAccessLinkIdentifiers, conf => conf.MapFrom(src => src.Ports.Select(port => port.GetPortName())));
        }
    }
}