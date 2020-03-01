using System.Linq;
using ChaosMonkey.Guards;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;
using DotNetNinja.KubernetesModule.Tests.Properties;
using Moq;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Domain
{
    public class KubernetesClusterTests
    {
        private const string Kubectl = "kubectl";
        private const string GetIngress = "get ingress -A";
        private static readonly string ExampleResponse = Resources.ExampleIngressData;
        private static readonly string BadExampleResponse = Resources.ExampleIngressDataWithInvalidRecord;
        public KubernetesClusterTests()
        {
            _mockShell = new Mock<ICommandShell>();
        }

        private readonly Mock<ICommandShell> _mockShell;

        [Fact]
        public void Instantiation_WithNullShell_ShouldThrowGuardException()
        {
            Assert.Throws<GuardException>(() => new KubernetesCluster(null));
        }

        [Fact]
        public void GetIngresses_ShouldExecuteKubectlCommand()
        {
            _mockShell.Setup(mock => mock.Execute(Kubectl, GetIngress)).Returns("");
            var cluster = new KubernetesCluster(_mockShell.Object);

            cluster.GetIngresses();

            _mockShell.Verify(mock=>mock.Execute(Kubectl, GetIngress), Times.Once);
        }

        [Fact]
        public void GetIngresses_WhenAllLinesAreValid_ShouldReturnExpectedValidResponses()
        {
            _mockShell.Setup(mock => mock.Execute(Kubectl, GetIngress)).Returns(ExampleResponse);
            var cluster = new KubernetesCluster(_mockShell.Object);

            var responses = cluster.GetIngresses();

            Assert.Equal(3, responses.Count());
        }


        [Fact]
        public void GetIngresses_WithInvalidLineInData_ShouldReturnExpectedValidResponses()
        {
            _mockShell.Setup(mock => mock.Execute(Kubectl, GetIngress)).Returns(BadExampleResponse);
            var cluster = new KubernetesCluster(_mockShell.Object);

            var responses = cluster.GetIngresses();

            Assert.Equal(2, responses.Count());
        }

    }
}