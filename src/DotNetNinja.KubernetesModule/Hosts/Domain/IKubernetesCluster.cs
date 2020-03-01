using System.Collections.Generic;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public interface IKubernetesCluster
    {
        IEnumerable<IngressResponse> GetIngresses();
    }
}