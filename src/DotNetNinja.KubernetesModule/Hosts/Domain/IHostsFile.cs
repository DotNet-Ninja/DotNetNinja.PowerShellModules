using System.Collections.Generic;
using System.Net;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public interface IHostsFile
    {
        IReadOnlyCollection<HostsEntry> Entries { get; }
        HostsUpdateResult Upsert(IPAddress address, string hostName, string comment = null);
        void Load();
        void Save();
    }
}