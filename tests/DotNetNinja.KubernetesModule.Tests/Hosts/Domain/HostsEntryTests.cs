using System.Net;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using Xunit;

namespace DotNetNinja.KubernetesModule.Tests.Hosts.Domain
{
    public class HostsEntryTests
    {
        private static readonly IPAddress TestAddress = IPAddress.Parse("192.168.1.1");
        private const string TestHostName = "www.example.com";
        private const string TestComment = "Example Comment";
        private static readonly string TestLineTabDelimited = $"{TestAddress}\t{TestHostName} # {TestComment}";
        private static readonly string TestLineSpaceDelimited = $"{TestAddress} {TestHostName} # {TestComment}";
        private static readonly string TestLineNoSpaceAroundComment = $"{TestAddress}\t{TestHostName}#{TestComment}";

        [Fact]
        public void Instantiation_ShouldInitializeAddress()
        {
            var entry = new HostsEntry(TestAddress, TestHostName, TestComment);

            Assert.Equal(TestAddress, entry.Address);
        }

        [Fact]
        public void Instantiation_ShouldInitializeAddressString()
        {
            var entry = new HostsEntry(TestAddress, TestHostName, TestComment);

            Assert.NotNull(entry.AddressString);
            Assert.Equal(TestAddress.ToString(), entry.AddressString);
        }

        [Fact]
        public void Instantiation_ShouldInitializeHostName()
        {
            var entry = new HostsEntry(TestAddress, TestHostName, TestComment);

            Assert.Equal(TestHostName, entry.HostName);
        }

        [Fact]
        public void Instantiation_ShouldInitializeComment()
        {
            var entry = new HostsEntry(TestAddress, TestHostName, TestComment);

            Assert.Equal(TestComment, entry.Comment);
        }

        [Fact]
        public void Instantiation_ShouldInitializeLine()
        {
            var entry = new HostsEntry(TestAddress, TestHostName, TestComment);

            Assert.Equal("192.168.1.1\twww.example.com # Example Comment", entry.Line);
        }

        [Fact]
        public void Instantiation_FromLine_ShouldInitializeAddress()
        {
            var entry = new HostsEntry(TestLineTabDelimited);

            Assert.Equal(TestAddress, entry.Address);
        }

        [Fact]
        public void Instantiation_FromLine_ShouldInitializeAddressString()
        {
            var entry = new HostsEntry(TestLineTabDelimited);

            Assert.Equal(TestAddress.ToString(), entry.AddressString);
        }

        [Fact]
        public void Instantiation_FromLine_ShouldInitializeHostName()
        {
            var entry = new HostsEntry(TestLineTabDelimited);

            Assert.Equal(TestHostName, entry.HostName);
        }

        [Fact]
        public void Instantiation_FromLine_ShouldInitializeComment()
        {
            var entry = new HostsEntry(TestLineTabDelimited);

            Assert.Equal(" " + TestComment, entry.Comment);
        }

        [Fact]
        public void Instantiation_FromLine_ShouldInitializeLine()
        {
            var entry = new HostsEntry(TestLineTabDelimited);

            Assert.Equal(TestLineTabDelimited, entry.Line);
        }

        // Inability of the parser to handle a line needs to result in the same line being
        // output if the file is rewritten! To handle this we save the original line read
        // to be rewritten, even if we don't understand it.  That way we should avoid
        // corrupting a hosts file due to a missed edge case that works.We also preserve
        // the formatting of the file as much as possible.
        [Fact]
        public void Instantiation_FromInvalidLine_ShouldInitializeLineWithoutChange()
        {
            const string invalid = "192.168.1.256\t #Comment";
            var entry = new HostsEntry(invalid);

            Assert.Equal(invalid, entry.Line);
        }

        [Fact]
        public void Instantiation_FromLineWithEmptyComment_ShouldInitializeCommentToEmptyString()
        {
            var entry = new HostsEntry($"{TestAddress}\t{TestHostName} #");

            Assert.Equal(string.Empty, entry.Comment);
        }

        [Fact]
        public void Instantiation_FromLineWithSpaceDelimiter_ShouldInitializeAddress()
        {
            var entry = new HostsEntry(TestLineSpaceDelimited);

            Assert.Equal(TestAddress, entry.Address);
        }

        [Fact]
        public void Instantiation_FromLineWithSpaceDelimiter_ShouldInitializeHostName()
        {
            var entry = new HostsEntry(TestLineSpaceDelimited);

            Assert.Equal(TestHostName, entry.HostName);
        }

        [Fact]
        public void Instantiation_FromLineWithNoSpaceAroundComment_ShouldInitializeHostName()
        {
            var entry = new HostsEntry(TestLineNoSpaceAroundComment);

            Assert.Equal(TestHostName, entry.HostName);
        }

        [Fact]
        public void Instantiation_FromLineWithNoSpaceAroundComment_ShouldInitializeComment()
        {
            var entry = new HostsEntry(TestLineNoSpaceAroundComment);

            Assert.Equal(TestComment, entry.Comment);
        }

        [Theory]
        [InlineData("www.example.com#Comment", "255.255.255.255")]
        [InlineData("192.1.1#Comment", "255.255.255.255")]
        [InlineData("192.1.1.1www.example.com", "255.255.255.255")]
        [InlineData("ABCDEFGHIJKLMNOP", "255.255.255.255")]
        public void Instantiation_WithVariousInvalidLines_ShouldReturnExpectedAddress(string line, string ip)
        {
            var expected = IPAddress.Parse(ip);

            var entry = new HostsEntry(line);

            Assert.Equal(expected, entry.Address);
        }

        [Theory]
        [InlineData("www.example.com#Comment", null)]
        [InlineData("192.1.1#Comment", null)]
        [InlineData("192.1.1.1www.example.com", null)]
        [InlineData("ABCDEFGHIJKLMNOP", null)]
        public void Instantiation_WithVariousInvalidLines_ShouldReturnExpectedAddressString(string line, string expected)
        {
            var entry = new HostsEntry(line);
            Assert.Equal(expected, entry.AddressString);
        }

        [Theory]
        [InlineData("www.example.com#Comment", null)]
        [InlineData("192.1.1#Comment", null)]
        [InlineData("192.1.1.1www.example.com", null)]
        [InlineData("ABCDEFGHIJKLMNOP", null)]
        public void Instantiation_WithVariousInvalidLines_ShouldReturnExpectedHostName(string line, string expected)
        {
            var entry = new HostsEntry(line);
            Assert.Equal(expected, entry.HostName);
        }

        [Theory]
        [InlineData("www.example.com#Comment", "Comment")]
        [InlineData("192.1.1#Comment", "Comment")]
        [InlineData("192.1.1.1www.example.com", null)]
        [InlineData("ABCDEFGHIJKLMNOP", null)]
        public void Instantiation_WithVariousInvalidLines_ShouldReturnExpectedComment(string line, string expected)
        {
            var entry = new HostsEntry(line);
            Assert.Equal(expected, entry.Comment);
        }

        [Theory]
        [InlineData("www.example.com#Comment")]
        [InlineData("192.1.1#Comment")]
        [InlineData("192.1.1.1www.example.com")]
        [InlineData("ABCDEFGHIJKLMNOP")]
        public void Instantiation_WithVariousInvalidLines_ShouldReturnExpectedLine(string line)
        {
            var entry = new HostsEntry(line);
            Assert.Equal(line, entry.Line);
        }

        [Fact]
        public void ToString_ShouldReturnLine()
        {
            var entry = new HostsEntry(TestLineSpaceDelimited);

            var result = entry.ToString();

            Assert.Equal(entry.Line, result);
        }
    }
}