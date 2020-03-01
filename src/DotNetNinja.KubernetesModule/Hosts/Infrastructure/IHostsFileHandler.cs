using System.Collections.Generic;

namespace DotNetNinja.KubernetesModule.Hosts.Infrastructure
{
    public interface IHostsFileHandler
    {
        List<string> Read();
        void Write(IEnumerable<string> lines);
    }
}