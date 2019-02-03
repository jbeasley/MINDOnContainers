using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class RouteDistinguisherRange : ValueObject
    {
        public string Name { get; private set; }
        public int AdministratorSubField { get; private set; }
        public int AssignedNumberSubFieldStartValue { get; private set; }
        public int AssignedNumberSubFieldEndValue { get; private set; }
        private readonly List<RoutingInstance> _routingInstances;
        public IReadOnlyCollection<RoutingInstance> RoutingInstances => _routingInstances;

        protected RouteDistinguisherRange()
        {
            _routingInstances = new List<RoutingInstance>();
        }

        public RouteDistinguisherRange(string name, int administratorSubField, int assignedNumberSubFieldStartValue, int assignedNumberSubFieldEndValue) : this()
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Name = name;

            if (administratorSubField < 1 || administratorSubField > 65535)
            {
                throw new AttachmentDomainException($"The administrator subfield value for route distinguisher range '{name}' must be between 1 and 65535.");
            }

            if (assignedNumberSubFieldStartValue < 1 || assignedNumberSubFieldStartValue > (2 ^ 32 - 1))
            {
                throw new AttachmentDomainException($"The start value for the assigned number subfield of route distinguisher range '{name}' must be between 1 and 4294967295.");
            }

            if (assignedNumberSubFieldEndValue < 2 || assignedNumberSubFieldEndValue > (2 ^ 32))
            {
                throw new AttachmentDomainException($"The end value for the assigned number subfield of route distinguisher range '{name}' must be between 2 and 4294967296.");
            }

            if (assignedNumberSubFieldStartValue >= assignedNumberSubFieldEndValue)
            {
                throw new AttachmentDomainException($"The start value for the assigned number subfield of route distinguishe range '{name}' must be less than the end value.");
            }

            AdministratorSubField = administratorSubField;
            AssignedNumberSubFieldStartValue = assignedNumberSubFieldStartValue;
            AssignedNumberSubFieldEndValue = assignedNumberSubFieldEndValue;
        }

        public int GetCount()
        {
            return AssignedNumberSubFieldEndValue - AssignedNumberSubFieldStartValue + 1;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return AdministratorSubField;
            yield return AssignedNumberSubFieldEndValue;
            yield return AssignedNumberSubFieldStartValue;
            yield return RoutingInstances;
        }
    }
}