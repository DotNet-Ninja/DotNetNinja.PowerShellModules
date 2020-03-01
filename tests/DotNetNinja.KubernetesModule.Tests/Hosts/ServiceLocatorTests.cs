using DotNetNinja.KubernetesModule.Hosts;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts
{
    public class ServiceLocatorTests
    {
        public ServiceLocatorTests()
        {
            _services = new ServiceLocator();
        }

        private readonly ServiceLocator _services;

        [Fact]
        public void GetIHostsFile_ReturnsHostsFile()
        {
            var result = _services.Get<IHostsFile>();

            Assert.IsType<HostsFile>(result);
        }

        [Fact]
        public void GetIKubernetesCluster_ReturnsKubernetesCluster()
        {
            var result = _services.Get<IKubernetesCluster>();

            Assert.IsType<KubernetesCluster>(result);
        }

        [Fact]
        public void GetICommandShell_ReturnsCommandShell()
        {
            var result = _services.Get<ICommandShell>();

            Assert.IsType<CommandShell>(result);
        }

        [Fact]
        public void GetIHostsFileHandler_ReturnsHostsFileHandler()
        {
            var result = _services.Get<IHostsFileHandler>();

            Assert.IsType<HostsFileHandler>(result);
        }

        [Fact]
        public void GetIFileStreamFactory_ReturnsFileStreamFactory()
        {
            var result = _services.Get<IStreamFactory>();

            Assert.IsType<FileStreamFactory>(result);
        }

    }
}