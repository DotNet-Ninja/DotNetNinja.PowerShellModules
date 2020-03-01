using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ChaosMonkey.Guards;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;
using DotNetNinja.KubernetesModule.Tests.Properties;
using Moq;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Domain
{
    public class HostsFileTests: IDisposable
    {
        private const string ExampleAddressString = "192.168.1.1";
        private const string AlternateAddressString = "192.168.1.2";
        private static readonly IPAddress ExampleAddress = IPAddress.Parse(ExampleAddressString);
        private static readonly IPAddress AlternateAddress = IPAddress.Parse(AlternateAddressString);
        private const string ExampleHostName = "www.example.com";
        private const string AlternateHostName = "alternate.example.com";
        private const string ExampleComment = "Example Comment";
        private const string AlternateComment = "New Comment";

        public HostsFileTests()
        {
            _mockHandler = new Mock<IHostsFileHandler>();
        }

        public void Dispose()
        {
            _mockHandler = null;
        }

        private Mock<IHostsFileHandler> _mockHandler;

        [Fact]
        public void Instantiation_WithNullFileHandler_ShouldThrowGuardException()
        {
            Assert.Throws<GuardException>(() => new HostsFile(null));
        }

        [Fact]
        public void Instantiation_ShouldInitializeEntriesToEmpty()
        {
            var hosts = new HostsFile(_mockHandler.Object);

            Assert.NotNull(hosts.Entries);
            Assert.Empty(hosts.Entries);
        }

        [Fact]
        public void Load_ShouldSetEntries()
        {
            _mockHandler.Setup(mock => mock.Read()).Returns(GetSampleFileData());
            var hosts = new HostsFile(_mockHandler.Object);

            hosts.Load();

            Assert.NotNull(hosts.Entries);
            Assert.Equal(23, hosts.Entries.Count);
        }

        [Fact]
        public void Load_ShouldClearHasUnsavedChanges()
        {
            _mockHandler.Setup(mock => mock.Read()).Returns(GetSampleFileData());
            var hosts = new HostsFile(_mockHandler.Object) {HasUnsavedChanges = true};

            hosts.Load();

            Assert.False(hosts.HasUnsavedChanges, "Expected HasUnsavedChanges to be false, but it was true.");
        }

        [Fact]
        public void Save_WhenHasUnsavedChanges_ShouldSaveFileUsingFileHandler()
        {
            var hosts = new HostsFile(_mockHandler.Object){HasUnsavedChanges = true};

            hosts.Save();

            _mockHandler.Verify(mock=>mock.Write(It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public void Save_WhenHasUnsavedChanges_ShouldClearHasUnsavedChanges()
        {
            var hosts = new HostsFile(_mockHandler.Object) { HasUnsavedChanges = true };

            hosts.Save();

            Assert.False(hosts.HasUnsavedChanges, "Expected HasUnsavedChanges to be false, but it was true.");
        }

        [Fact]
        public void Upsert_WithNullHostName_ShouldThrowGuardException()
        {
            var hosts = new HostsFile(_mockHandler.Object);

            Assert.Throws<GuardException>(() => hosts.Upsert(IPAddress.None, null));
        }

        [Fact]
        public void Upsert_WhenEntryWithHostNameExistsWithSameAddress_ShouldNotReplaceEntry()
        {
            var hosts = GetHostsFileWithEntries();

            hosts.Upsert(ExampleAddress, ExampleHostName, AlternateComment);

            Assert.Equal(ExampleComment, hosts.Entries.Single(entry => entry.HostName == ExampleHostName).Comment);
        }

        [Fact]
        public void Upsert_WhenEntryWithHostNameExistsWithSameAddress_ShouldReturnUnchangedResultStatus()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(ExampleAddress, ExampleHostName);

            Assert.Equal(HostsUpdateResultType.Unchanged, result.Status);
        }

        [Fact]
        public void Upsert_WhenEntryWithHostNameExistsWithSameAddress_ShouldReturnResultWithExpectedOldAddress()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(ExampleAddress, ExampleHostName);

            Assert.Equal(ExampleAddress, result.OldAddress);
        }

        [Fact]
        public void Upsert_WhenEntryWithHostNameExistsWithSameAddress_ShouldReturnResultIdenticalOldAndNewAddress()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(ExampleAddress, ExampleHostName);

            Assert.Equal(result.NewAddress, result.OldAddress);
        }

        [Fact]
        public void Upsert_WhenEntryWithHostNameExistsWithSameAddress_ShouldNotSetHasUnsavedChanges()
        {
            var hosts = GetHostsFileWithEntries();
            hosts.HasUnsavedChanges = false;

            hosts.Upsert(ExampleAddress, ExampleHostName);

            Assert.False(hosts.HasUnsavedChanges, "Expected HasUnsavedChanges to be false, but it was true.");
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameDoesNotExist_ShouldReturnAddedResultStatus()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, AlternateHostName, AlternateComment);

            Assert.Equal(HostsUpdateResultType.Added, result.Status);
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameDoesNotExist_ShouldReturnResultWithExpectedNewAddress()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, AlternateHostName, AlternateComment);

            Assert.Equal(AlternateAddress, result.NewAddress);
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameDoesNotExist_ShouldReturnResultWithExpectedOldAddress()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, AlternateHostName, AlternateComment);

            Assert.Equal(IPAddress.None, result.OldAddress);
        }
        
        [Fact]
        public void Upsert_WhenEntryForHostNameDoesNotExist_ShouldReturnResultWithExpectedHostName()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, AlternateHostName, AlternateComment);

            Assert.Equal(AlternateHostName, result.HostName);
        }
        
        [Fact]
        public void Upsert_WhenEntryForHostNameDoesNotExist_ShouldSetHasUnsavedChanges()
        {
            var hosts = GetHostsFileWithEntries();

            hosts.Upsert(AlternateAddress, AlternateHostName, AlternateComment);

            Assert.True(hosts.HasUnsavedChanges, "Expected HasUnsavedChanges to be true, but it was false.");
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameHasDifferentAddress_ShouldReturnUpdatedResultStatus()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, ExampleHostName, AlternateComment);

            Assert.Equal(HostsUpdateResultType.Updated, result.Status);
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameHasDifferentAddress_ShouldReturnExpectedOldAddress()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, ExampleHostName, AlternateComment);

            Assert.Equal(ExampleAddress, result.OldAddress);
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameHasDifferentAddress_ShouldReturnExpectedNewAddress()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, ExampleHostName, AlternateComment);

            Assert.Equal(AlternateAddress, result.NewAddress);
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameHasDifferentAddress_ShouldReturnExpectedHostName()
        {
            var hosts = GetHostsFileWithEntries();

            var result = hosts.Upsert(AlternateAddress, ExampleHostName, AlternateComment);

            Assert.Equal(ExampleHostName, result.HostName);
        }

        [Fact]
        public void Upsert_WhenEntryForHostNameHasDifferentAddress_ShouldSetHasUnsavedChanges()
        {
            var hosts = GetHostsFileWithEntries();

            hosts.Upsert(AlternateAddress, ExampleHostName, AlternateComment);

            Assert.True(hosts.HasUnsavedChanges, "Expected HasUnsavedChanges to be true, but it was false.");
        }

        private List<string> GetSampleFileData()
        {
            return Resources.SampleFileData.Split(new[] {'\r'}).Select(line => line.TrimEnd()).ToList();
        }

        private HostsFile GetHostsFileWithEntries(Mock<IHostsFileHandler> handler = null, List<HostsEntry> entries = null)
        {
            if (handler == null)
            {
                handler = _mockHandler = new Mock<IHostsFileHandler>();
            }
            var hosts = new HostsFile(handler.Object);
            if (entries == null)
            {
                entries = new List<HostsEntry>
                {
                    new HostsEntry(ExampleAddress, ExampleHostName, ExampleComment)
                };
            }
            hosts.InternalEntries = entries;
            return hosts;
        }
    }
}