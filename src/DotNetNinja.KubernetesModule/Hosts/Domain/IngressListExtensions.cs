using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public static class IngressListExtensions
    {
        public static IEnumerable<Ingress> GetValidIngresses(this IEnumerable<IngressResponse> responses)
        {
            return responses
                .Where(response => response.IsValid && !response.IsHeader)
                .Select(response => new Ingress(
                    response.Namespace.Trim(),
                    response.Name.Trim(),
                    response.Hosts.Trim(),
                    (IPAddress.TryParse(response.Address.Trim(), out var parsedAddress))
                        ? parsedAddress
                        : IPAddress.None,
                    response.Ports.Trim().Split(new []{':'}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(port => (int.TryParse(port, out var parsedPort) 
                            ? parsedPort 
                            : 0)).ToArray(),
                    response.Age.Trim()
                )).Where(ingress=>ingress.Ports.Contains(80)||ingress.Ports.Contains(443));
        }
    }
}