using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using DotNetNinja.KubernetesModule.Tests.Properties;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Domain
{
    public class IngressListExtensionsTests
    {
        [Fact]
        public void GetValidIngresses_ShouldRemoveHeaderResponses()
        {
            var responses = GetExampleIngressResponses();

            var ingresses = responses.GetValidIngresses().ToList();

            Assert.Equal(2, ingresses.Count());
            Assert.DoesNotContain(ingresses, ingress => ingress.Namespace == "NAMESPACE");
        }

        [Fact]
        public void GetValidIngresses_ShouldRemoveInvalidResponses()
        {
            var responses = GetExampleWithInvalidResponses();

            var ingresses = responses.GetValidIngresses().ToList();

            Assert.Single(ingresses);
            Assert.DoesNotContain(ingresses, ingress => ingress.Name == "dashboard");
        }

        [Fact]
        public void GetValidIngresses_ShouldSetExpectedNamespace()
        {
            var responses = GetExampleWithSingleRecord();

            var ingress = responses.GetValidIngresses().First();

            Assert.Equal("istio-system", ingress.Namespace);
        }
        
        [Fact]
        public void GetValidIngresses_ShouldSetExpectedName()
        {
            var responses = GetExampleWithSingleRecord();

            var ingress = responses.GetValidIngresses().First();

            Assert.Equal("kiali", ingress.Name);
        }

        [Fact]
        public void GetValidIngresses_ShouldSetExpectedHosts()
        {
            var responses = GetExampleWithSingleRecord();

            var ingress = responses.GetValidIngresses().First();

            Assert.Equal("kiali.minikube.local", ingress.Hosts);
        }

        [Fact]
        public void GetValidIngresses_ShouldSetExpectedAddress()
        {
            var expected = IPAddress.Parse("192.168.43.51");
            var responses = GetExampleWithSingleRecord();

            var ingress = responses.GetValidIngresses().First();

            Assert.Equal(expected, ingress.Address);
        }

        [Fact]
        public void GetValidIngresses_ShouldSetExpectedPorts()
        {
            var responses = GetExampleWithSingleRecord();

            var ingress = responses.GetValidIngresses().First();

            Assert.Equal(new []{80}, ingress.Ports);
        }

        [Fact]
        public void GetValidIngresses_ShouldSetExpectedAge()
        {
            var responses = GetExampleWithSingleRecord();

            var ingress = responses.GetValidIngresses().First();

            Assert.Equal("3d23h", ingress.Age);
        }

        [Fact]
        public void GetValidIngress_ShouldRemoveNonStandardHttpPorts()
        {
            var responses = GetExampleWithNonStandardPort();

            var ingresses = responses.GetValidIngresses();

            Assert.DoesNotContain(ingresses, ingress=>ingress.Ports.Contains(8443));
        }

        [Fact]
        public void GetValidIngress_ShouldReturnIngressesWithPort80()
        {
            var responses = GetExampleWithNonStandardPort();

            var ingresses = responses.GetValidIngresses();

            Assert.Contains(ingresses, ingress => ingress.Ports.Contains(80));
        }

        [Fact]
        public void GetValidIngress_ShouldReturnIngressesWithPort443()
        {
            var responses = GetExampleWithNonStandardPort();

            var ingresses = responses.GetValidIngresses();

            Assert.Contains(ingresses, ingress => ingress.Ports.Contains(443));
        }

        private IEnumerable<IngressResponse> GetExampleWithNonStandardPort()
        {
            return GetIngressResponses(Resources.ExampleIngressWithNonStandardPorts);
        }

        private IEnumerable<IngressResponse> GetExampleIngressResponses()
        {
            return GetIngressResponses(Resources.ExampleIngressData);
        }

        private IEnumerable<IngressResponse> GetExampleWithInvalidResponses()
        {
            return GetIngressResponses(Resources.ExampleIngressDataWithInvalidRecord);
        }

        private IEnumerable<IngressResponse> GetExampleWithSingleRecord()
        {
            return GetIngressResponses(Resources.ExampleIngressWithSingleRecord);
        }

        private IEnumerable<IngressResponse> GetIngressResponses(string text)
        {
            return text.Split('\r').Select(line => new IngressResponse(line));
        }
    }
}