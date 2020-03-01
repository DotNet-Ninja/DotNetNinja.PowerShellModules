using System.Collections.Generic;
using System.Linq;
using ChaosMonkey.Guards;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public class KubernetesCluster : IKubernetesCluster
    {
        public KubernetesCluster(ICommandShell shell)
        {
            Guard.IsNotNull(shell, nameof(shell));
            Shell = shell;
        }

        protected ICommandShell Shell { get; }

        public IEnumerable<IngressResponse> GetIngresses()
        {
            var delimiters = new [] {'\r', '\n'};
            var data = Shell.Execute("kubectl", "get ingress -A");
            var lines = data?.Split(delimiters)?.Where(line => line.Trim().Length > 0) ?? new List<string>();
            return lines.Select(line => new IngressResponse(line)).Where(response=>response.IsValid);
        }
    }
}