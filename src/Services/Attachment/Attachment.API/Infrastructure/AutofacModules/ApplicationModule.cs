using Autofac;
using MINDOnContainers.BuildingBlocks.EventBus.Abstractions;
using MINDOnContainers.Services.Attachment.API.Application.Commands;
using MINDOnContainers.Services.Attachment.API.Application.Queries;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;
using MINDOnContainers.Services.Attachment.Infrastructure.Idempotency;
using MINDOnContainers.Services.Attachment.Infrastructure.Repositories;
using MINDOnContainers.Services.AttachmentRole.Infrastructure.Repositories;
using System.Reflection;

namespace MINDOnContainers.Services.Attachment.API.Infrastructure.AutofacModules
{

    public class ApplicationModule
        :Autofac.Module
    {

        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new AttachmentQueries(QueriesConnectionString))
                .As<IAttachmentQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AttachmentRepository>()
                .As<IAttachmentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AttachmentRoleRepository>()
                .As<IAttachmentRoleRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CreateAttachmentCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

        }
    }
}
