using System.Net;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public class HostsUpdateResult
    {
        private HostsUpdateResult(string hostName)
        {
            HostName = hostName;
            OldAddress = IPAddress.None;
            NewAddress = IPAddress.None;
            Status = HostsUpdateResultType.Unchanged;
        }

        public string HostName { get; }
        public HostsUpdateResultType Status { get; private set; }
        public IPAddress OldAddress { get; private set; }
        public IPAddress NewAddress { get; private set; }

        public static HostsUpdateResult NoChange(HostsEntry entry)
        {
            return new HostsUpdateResult(entry.HostName)
            {
                Status = HostsUpdateResultType.Unchanged,
                NewAddress = entry.Address,
                OldAddress = entry.Address
            };
        }

        public static HostsUpdateResult Added(HostsEntry entry)
        {
            return new HostsUpdateResult(entry.HostName)
            {
                Status = HostsUpdateResultType.Added,
                NewAddress = entry.Address,
                OldAddress = IPAddress.None
            };
        }

        public static HostsUpdateResult Updated(HostsEntry entry, IPAddress newAddress)
        {
            return new HostsUpdateResult(entry.HostName)
            {
                Status = HostsUpdateResultType.Updated,
                NewAddress = newAddress,
                OldAddress = entry.Address
            };
        }
    }
}