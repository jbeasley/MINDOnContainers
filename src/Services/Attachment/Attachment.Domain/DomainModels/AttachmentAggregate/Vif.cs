using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Vif : Entity
    {
        private readonly int _attachmentId;
        public string Name { get; private set; }
        private readonly bool _isLayer3;
        public int VlanTag { get; private set; }
        private readonly int? _tenantId;
        private readonly bool _created;
        private readonly int? _routingInstanceId;
        private readonly RoutingInstance _routingInstance;
        private readonly int? _contractBandwidthPoolId;
        public ContractBandwidthPool ContractBandwidthPool { get; private set; }
        private readonly int _vifRoleId;
        private readonly VifRole _vifRole;
        private readonly int _mtuId;
        private readonly Mtu _mtu;
        private readonly List<Vlan> _vlans;
        public IReadOnlyCollection<Vlan> Vlans => _vlans;
        private readonly int _networkStatusId;

        protected Vif()
        {
            _vlans = new List<Vlan>();
            _created = true;
            _networkStatusId = NetworkStatus.Init.Id;
        }

        public Vif(bool isLayer3, VifRole role, Mtu mtu, List<Vlan> vlans, int vlanTag, RoutingInstance routingInstance = null, 
            ContractBandwidthPool contractBandwidthPool = null, int? tenantId = null, bool trustReceivedCosAndDscp = false) : this()
        {
            this.Name = Guid.NewGuid().ToString("N");
            this._vifRole = role ?? throw new ArgumentNullException(nameof(role));
            this._vifRoleId = role.Id;

            // Must have a tenant specified for a tenant-facing vif
            if (role.IsTenantFacing)
            {
                if (!this._tenantId.HasValue)
                {
                    throw new ArgumentNullException(nameof(tenantId));
                }

                this._tenantId = tenantId;
            }

            this._isLayer3 = isLayer3;
            this.VlanTag = vlanTag;
            this._mtu = mtu ?? throw new ArgumentNullException(nameof(mtu));
            this._mtuId = mtu.Id;

            if (role.RequireContractBandwidth)
            {
                this.ContractBandwidthPool = contractBandwidthPool ?? throw new ArgumentNullException(nameof(contractBandwidthPool));
                this._contractBandwidthPoolId = contractBandwidthPool.Id;
            }

            if (role.RequireRoutingInstance)
            {
                this._routingInstance = routingInstance ?? throw new ArgumentNullException(nameof(routingInstance));
                this._routingInstanceId = routingInstance.Id;
            }
        }
    }
}