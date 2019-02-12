using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Vif : Entity
    {
        private readonly int _attachmentId;
        public string Name { get; private set; }
        public int VlanTag { get; private set; }
        private readonly int? _tenantId;
        private readonly int? _contractBandwidthPoolId;
        public ContractBandwidthPool ContractBandwidthPool { get; private set; }
        private readonly int _vifRoleId;
        private readonly int _mtuId;
        private readonly Mtu _mtu;
        private readonly List<Vlan> _vlans;
        public IReadOnlyCollection<Vlan> Vlans => _vlans;
        private readonly int _networkStatusId;

        protected Vif()
        {
            _vlans = new List<Vlan>();
            _networkStatusId = NetworkStatus.Init.Id;
        }

        public Vif(int attachmentId, int tenantId, VifRole role, Mtu mtu, List<Vlan> vlans, int vlanTag,
            ContractBandwidthPool contractBandwidthPool = null, bool trustReceivedCosAndDscp = false) : this()
        {
            this._attachmentId = attachmentId;
            this.Name = Guid.NewGuid().ToString("N");
            this._vifRoleId = role.Id;
            this._tenantId = tenantId;
            this.VlanTag = vlanTag;
            this._mtu = mtu ?? throw new ArgumentNullException(nameof(mtu));
            this._mtuId = mtu.Id;

            if (role.RequireContractBandwidth)
            {
                this.ContractBandwidthPool = contractBandwidthPool ?? throw new ArgumentNullException(nameof(contractBandwidthPool));
                this._contractBandwidthPoolId = contractBandwidthPool.Id;
            }
        }
    }
}