using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class VlanTagRange : ValueObject
    {
        public string Name { get; private set; }
        public int StartValue { get; private set; }
        public int EndValue { get; private set; }

        private VlanTagRange()
        {
        }

        public VlanTagRange(string name, int startValue, int endValue)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Name = name;

            if (startValue < 2 || startValue > 4093)
            {
                throw new AttachmentDomainException($"The start value for a vlan tag range '{name}' must be between 2 and 4093.");
            }

            if (endValue < 3 || endValue > 4094)
            {
                throw new AttachmentDomainException($"The end value for vlan tag range '{name}' must be between 3 and 4094.");
            }

            if (startValue >= endValue)
            {
                throw new AttachmentDomainException($"The start value for vlan tag range '{name}' must be less than the end value.");
            }

            StartValue = startValue;
            EndValue = endValue;

        }

        public int GetCount()
        {
            return EndValue - StartValue + 1;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return StartValue;
            yield return EndValue;
        }
    }
}