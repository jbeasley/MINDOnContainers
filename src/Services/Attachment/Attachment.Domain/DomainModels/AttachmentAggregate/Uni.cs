using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Uni : Entity
    {
        public string Name { get; private set; }
        private readonly List<string> _uniAccessLinkIdentifiers;
        public IReadOnlyCollection<string> UniAccessLinkIdentifiers;
        public int? RoutingInstanceId { get; private set; }
        private readonly List<BgpPeer> _bgpPeers;
        public IReadOnlyCollection<BgpPeer> BgpPeers;

        protected Uni()
        {
            this._uniAccessLinkIdentifiers = new List<string>();
            this._bgpPeers = new List<BgpPeer>();
        }

        public Uni(string name, List<string> uniAccessLinkIdentifiers, List<BgpPeer> bgpPeers = null, int? routingInstanceId = null) : this()
        {
            this.Name = name;
            this.UniAccessLinkIdentifiers = uniAccessLinkIdentifiers;
            this.RoutingInstanceId = routingInstanceId;
            if (bgpPeers != null) this._bgpPeers = bgpPeers;
        }
    }
}
