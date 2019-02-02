using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class RouteDistinguisherRange : ValueObject
    {
        public string Name { get; private set; }
        public int AdministratorSubField { get; private set; }
        private readonly int _assignedNumberSubFieldStartValue;
        private readonly int _assignedNumberSubFieldEndValue;
        private readonly List<RoutingInstance> _routingInstances;
        public IReadOnlyCollection<RoutingInstance> RoutingInstances => _routingInstances;
    }

    public RouteDistinguisherRange(string name, int administratorSubField, int assignedNumberSubFieldStartValue, int assignedNumberSubFieldEndValue)
    {
        !string.IsNullOrEmpty(name) ? Name = name : throw new NullValueException("A name is required to create a new route distinguishe range.");

        if (administratorSubField < 1 || administratorSubField > 65535)
        {
            throw new AttachmentDomainException($"The administrator subfield value for route distinguisher range '{name}' must be between 1 and 65535.");
        }

        if (assignedNumberSubFieldStartValue < 1 || assignedNumberSubFieldStartValue > 2^32 - 1)
        {
            throw new AttachmentDomainException($"The start value for the assigned number subfield of route distinguisher range '{name}' must be between 1 and 4294967295.");
        }

        if (assignedNumberSubFieldEndValue < 2 || assignedNumberSubFieldEndValue > 2^32)
        {
            throw new AttachmentDomainException($"The end value for the assigned number subfield of route distinguisher range '{name}' must be between 2 and 4294967296.");
        }

        if (assignedNumberSubFieldStartValue >= assignedNumberSubFieldEndValue)
        {
            throw new AttachmentDomainException($"The start value for the assigned number subfield of route distinguishe range '{name}' must be less than the end value.");
        }

        _administratorSubField = administratorSubField;
        _assignedNumberSubFieldStartValue = assignedNumberSubFieldStartValue;
        _assignedNumberSubFieldEndValue = assignedNumberSubFieldEndValue;
    }
}