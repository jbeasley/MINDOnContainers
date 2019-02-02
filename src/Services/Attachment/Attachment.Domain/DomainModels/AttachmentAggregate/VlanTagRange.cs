using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class VlanTagRange : ValueObject
    {
        public string Name { get; private set; }
        private readonly int _startValue;
        private readonly int _endValue;
    }

    public VlanTagRange(string name, int startValue, int endValue)
    {
        !string.IsNullOrEmpty(name) ? Name = name : throw new NullValueException("A name is required to create a new vlan tag range.");

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

        _startValue = startValue;
        _endValue = endValue;

    }
}