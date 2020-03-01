using System;
using System.Net;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public class Ingress
    {
        public Ingress(string @namespace, string name, string hosts, IPAddress address, int[] ports, string age)
        {
            Namespace = @namespace;
            Name = name;
            Hosts = hosts;
            Address = address;
            Ports = ports;
            Age = age;
        }

        public string Namespace { get; }
        public string Name { get; }
        public string Hosts { get; }
        public IPAddress Address { get; }
        public int[] Ports { get; }
        public string Age { get; }
    }
}