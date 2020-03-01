using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;
using DotNetNinja.KubernetesModule.Tests.Properties;
using Moq;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Infrastructure
{
    public class HostsFileHandlerTests : IDisposable
    {
        public HostsFileHandlerTests()
        {
            _mockFactory = new Mock<IStreamFactory>();
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }

        private MemoryStream _stream;
        private readonly Mock<IStreamFactory> _mockFactory;

        [Fact]
        public void Read_ShouldCreateReadOnlyStream()
        {
            _stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.SampleFileData));
            _mockFactory.Setup(mock => mock.CreateReadOnlyShared(It.IsAny<string>())).Returns(_stream);
            var handler = new HostsFileHandler(_mockFactory.Object);

            handler.Read();

            _mockFactory.Verify(mock => mock.CreateReadOnlyShared(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Read_ShouldReturnAllLinesInStream()
        {
            _stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.SampleFileData));
            _mockFactory.Setup(mock => mock.CreateReadOnlyShared(It.IsAny<string>())).Returns(_stream);
            var handler = new HostsFileHandler(_mockFactory.Object);

            var lines = handler.Read();

            Assert.Equal(23, lines.Count);
        }

        [Fact]
        public void Write_ShouldCreateWriteOnlyStream()
        {
            _stream = new MemoryStream();
            _mockFactory.Setup(mock => mock.CreateWriteOnlyShared(It.IsAny<string>())).Returns(_stream);
            var handler = new HostsFileHandler(_mockFactory.Object);

            handler.Write(new List<string> { "Line 1", "Line 2" });

            _mockFactory.Verify(mock => mock.CreateWriteOnlyShared(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Write_ShouldWriteExpectedLines()
        {
            _stream = new MemoryStream();
            _mockFactory.Setup(mock => mock.CreateWriteOnlyShared(It.IsAny<string>())).Returns(_stream);
            var handler = new HostsFileHandler(_mockFactory.Object);

            handler.Write(new List<string> { "Line 1", "Line 2" });

            var written = Encoding.UTF8.GetString(_stream.ToArray());
        }

    }
}