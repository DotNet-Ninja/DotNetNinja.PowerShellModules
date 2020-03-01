using System;
using System.Net;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public class HostsEntry
    {
        public HostsEntry(string hostsLine)
        {
            FromLine(hostsLine);
        }

        public HostsEntry(IPAddress address, string hostName, string comment = null)
        {
            Address = address;
            AddressString = address.ToString();
            HostName = hostName;
            Comment = comment;
            var commentText = (Comment != null) ? $"# {Comment}" : string.Empty;
            Line = $"{Address}\t{HostName} {commentText}".Trim();
        }

        public static readonly char[] Delimiters = {'\t', ' '};

        public string Comment { get; private set; }
        public string HostName { get; private set; }
        public IPAddress Address { get; private set; }
        public string AddressString { get; private set; }
        public string Line { get; private set; }

        private void FromLine(string hostLine)
        {
            Line = hostLine;
            var workingLine = hostLine.Trim();
            var commentStart = hostLine.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
            if (commentStart >= 0)
            {
                Comment = hostLine.Substring(commentStart + 1);
                workingLine = hostLine.Substring(0, commentStart).Trim();
            }
            var components = workingLine.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
            Address = IPAddress.None;
            HostName = null;
            AddressString = null;
            if (components.Length == 2)
            {
                AddressString = components[0];
                if (IPAddress.TryParse(AddressString, out var address))
                {
                    Address = address;
                }
                HostName = components[1];
            }
        }

        public override string ToString()
        {
            return Line;
        }
    }
}