using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class RoutingInstance : Entity
    {
        public string Name { get; private set; }
        public int? AdministratorSubField { get; private set; }
        public int? AssignedNumberSubField { get; private set; }
        private readonly int _deviceId;
        private readonly List<Vif> _vifs;
        public IReadOnlyCollection<Vif> Vifs => _vifs;
        private readonly List<Attachment> _attachments;
        public IReadOnlyCollection<Attachment> Attachments => _attachments;
        private readonly List<BgpPeer> _bgpPeers;
        public IReadOnlyCollection<BgpPeer> BgpPeers => _bgpPeers;

        protected RoutingInstance()
        {
            _bgpPeers = new List<BgpPeer>();
            _vifs = new List<Vif>();
            _attachments = new List<Attachment>();
        }

        public RoutingInstance(int deviceId, string name, 
        int? administratorSubField = null, int? assignedNumberSubField = null) : this()
        {
            this._deviceId = deviceId;
        }         
    }
}