using System;
using System.Linq;
using System.Net;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Domain
{
    public class IngressTests
    {
        private static readonly string ExampleNamespace = Guid.NewGuid().ToString();
        private static readonly string ExampleName = Guid.NewGuid().ToString();
        private static readonly string ExampleHosts= Guid.NewGuid().ToString();
        private static readonly IPAddress ExampleAddress = IPAddress.Parse("127.0.0.1");
        private static readonly int[] ExamplePorts = { 80, 443};
        private static readonly string ExampleAge = Guid.NewGuid().ToString();

        [Fact]
        public void Instantiation_ShouldInitializeNamespaceToPassedValue()
        {
            var ingress = new Ingress(ExampleNamespace, ExampleName, ExampleHosts, ExampleAddress, ExamplePorts, ExampleAge);

            Assert.Equal(ExampleNamespace, ingress.Namespace);
        }

        [Fact]
        public void Instantiation_ShouldInitializeNameToPassedValue()
        {
            var ingress = new Ingress(ExampleNamespace, ExampleName, ExampleHosts, ExampleAddress, ExamplePorts, ExampleAge);

            Assert.Equal(ExampleName, ingress.Name);
        }

        [Fact]
        public void Instantiation_ShouldInitializeHostsToPassedValue()
        {
            var ingress = new Ingress(ExampleNamespace, ExampleName, ExampleHosts, ExampleAddress, ExamplePorts, ExampleAge);

            Assert.Equal(ExampleHosts, ingress.Hosts);
        }

        [Fact]
        public void Instantiation_ShouldInitializeAddressToPassedValue()
        {
            var ingress = new Ingress(ExampleNamespace, ExampleName, ExampleHosts, ExampleAddress, ExamplePorts, ExampleAge);

            Assert.Equal(ExampleAddress, ingress.Address);
        }

        [Fact]
        public void Instantiation_ShouldInitializePortsToPassedValue()
        {
            var ingress = new Ingress(ExampleNamespace, ExampleName, ExampleHosts, ExampleAddress, ExamplePorts, ExampleAge);

            Assert.Equal(ExamplePorts.Length, ingress.Ports.Length);
            Assert.Empty(ingress.Ports.Except(ExamplePorts));
        }

        [Fact]
        public void Instantiation_ShouldInitializeAgeToPassedValue()
        {
            var ingress = new Ingress(ExampleNamespace, ExampleName, ExampleHosts, ExampleAddress, ExamplePorts, ExampleAge);

            Assert.Equal(ExampleAge, ingress.Age);
        }
    }
}