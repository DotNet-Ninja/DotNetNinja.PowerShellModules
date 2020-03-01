using System;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Domain
{
    public class IngressResponseTests
    {
        private const string ExampleHeaderText = "NAMESPACE              NAME        HOSTS                      ADDRESS         PORTS   AGE";
        private const string ExampleRecordText = "kubernetes-dashboard   dashboard   dashboard.minikube.local   192.168.43.51   80      3d23h";
        private const string BadExampleRecordText = "kubernetes-dashboard   dashboard   dashboard.minikube.local   192.168.43.51   80"; // Only 5 items

        [Fact]
        public void Instantiation_WithNullLine_ShouldSetRawToEmptyString()
        {
            var response = new IngressResponse(null);

            Assert.NotNull(response.Raw);
            Assert.Empty(response.Raw);
        }

        [Fact]
        public void Instantiation_WithBadLine_ShouldSetIsValidFalse()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.False(response.IsValid, "Expected IsValid to be false, but was true.");
        }

        [Fact]
        public void Instantiation_WithBadLine_ShouldSetIsHeaderFalse()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.False(response.IsHeader, "Expected IsHeader to be false, but was true.");
        }

        [Fact]
        public void Instantiation_WithBadLine_ShouldSetNamespaceNull()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.Null(response.Namespace);
        }

        [Fact]
        public void Instantiation_WithBadLine_ShouldSetNameNull()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.Null(response.Name);
        }
        
        [Fact]
        public void Instantiation_WithBadLine_ShouldSetHostsNull()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.Null(response.Hosts);
        }

        [Fact]
        public void Instantiation_WithBadLine_ShouldSetAddressNull()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.Null(response.Address);
        }

        [Fact]
        public void Instantiation_WithBadLine_ShouldSetPortsNull()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.Null(response.Ports);
        }

        [Fact]
        public void Instantiation_WithBadLine_ShouldSetAgeNull()
        {
            var response = new IngressResponse(BadExampleRecordText);

            Assert.Null(response.Age);
        }
        
        [Fact]
        public void Instantiation_WithValidLine_ShouldSetIsValidTrue()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.True(response.IsValid, "Expected IsValid to be true, but was false.");
        }

        [Fact]
        public void Instantiation_WithValidLine_ShouldSetIsHeaderFalse()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.False(response.IsHeader, "Expected IsHeader to be false, but was true.");
        }

        [Fact]
        public void Instantiation_WithValidLine_ShouldSetNamespace()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.Equal("kubernetes-dashboard", response.Namespace);
        }

        [Fact]
        public void Instantiation_WithValidLine_ShouldSetName()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.Equal("dashboard", response.Name);
        }

        [Fact]
        public void Instantiation_WithValidLine_ShouldSetHosts()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.Equal("dashboard.minikube.local", response.Hosts);
        }

        [Fact]
        public void Instantiation_WithValidLine_ShouldSetAddress()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.Equal("192.168.43.51", response.Address);
        }

        [Fact]
        public void Instantiation_WithValidLine_ShouldSetPorts()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.Equal("80", response.Ports);
        }

        [Fact]
        public void Instantiation_WithValidLine_ShouldSetAge()
        {
            var response = new IngressResponse(ExampleRecordText);

            Assert.Equal("3d23h", response.Age);
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetIsValidTrue()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.True(response.IsValid, "Expected IsValid to be true, but was false.");
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetIsHeaderTrue()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.True(response.IsHeader, "Expected IsHeader to be true, but was false.");
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetNamespace()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.Equal("NAMESPACE", response.Namespace);
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetName()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.Equal("NAME", response.Name);
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetHosts()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.Equal("HOSTS", response.Hosts);
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetAddress()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.Equal("ADDRESS", response.Address);
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetPorts()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.Equal("PORTS", response.Ports);
        }

        [Fact]
        public void Instantiation_WithHeaderLine_ShouldSetAge()
        {
            var response = new IngressResponse(ExampleHeaderText);

            Assert.Equal("AGE", response.Age);
        }

        [Fact]
        public void ToString_ShouldReturnRaw()
        {
            var line = Guid.NewGuid().ToString();

            var response = new IngressResponse(line);

            Assert.Equal(line, response.Raw);
            Assert.Equal(response.Raw, response.ToString());
        }
    }
}