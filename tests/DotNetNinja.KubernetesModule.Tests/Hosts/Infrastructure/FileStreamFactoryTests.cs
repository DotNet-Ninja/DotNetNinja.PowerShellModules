using System.IO;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Infrastructure
{
    public class FileStreamFactoryTests
    {
        [Fact]
        public void CreateWriteOnlyShared_ShouldReturnFileStream()
        {
            var factory = new FileStreamFactory();

            var result = factory.CreateWriteOnlyShared(Paths.SampleFile);

            Assert.IsType<FileStream>(result);
        }

        [Fact]
        public void CreateWriteOnlyShared_ShouldReturnWritableFileStream()
        {
            var factory = new FileStreamFactory();

            var result = factory.CreateWriteOnlyShared(Paths.SampleFile);

            Assert.True(result.CanWrite, "The returned stream was not writable.");
        }

        [Fact]
        public void CreateWriteOnlyShared_ShouldReturnUnreadableFileStream()
        {
            var factory = new FileStreamFactory();

            var result = factory.CreateWriteOnlyShared(Paths.SampleFile);

            Assert.False(result.CanRead, "The returned stream was readable.");
        }

        [Fact]
        public void CreateReadOnlyShared_ShouldReturnFileStream()
        {
            var factory = new FileStreamFactory();

            var result = factory.CreateReadOnlyShared(Paths.SampleFile);

            Assert.IsType<FileStream>(result);
        }

        [Fact]
        public void CreateReadOnlyShared_ShouldReturnWritableFileStream()
        {
            var factory = new FileStreamFactory();

            var result = factory.CreateReadOnlyShared(Paths.SampleFile);

            Assert.True(result.CanRead, "The returned stream was not readable.");
        }

        [Fact]
        public void CreateReadOnlyShared_ShouldReturnUnwritableFileStream()
        {
            var factory = new FileStreamFactory();

            var result = factory.CreateReadOnlyShared(Paths.SampleFile);

            Assert.False(result.CanWrite, "The returned stream was writable.");
        }
    }
}