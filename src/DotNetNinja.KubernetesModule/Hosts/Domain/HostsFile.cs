using System.Collections.Generic;
using System.Linq;
using System.Net;
using ChaosMonkey.Guards;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public class HostsFile : IHostsFile
    {
        public HostsFile(IHostsFileHandler fileHandler)
        {
            Guard.IsNotNull(fileHandler, nameof(fileHandler));
            FileHandler = fileHandler;
            InternalEntries = new List<HostsEntry>();
        }

        internal List<HostsEntry> InternalEntries;
        public IReadOnlyCollection<HostsEntry> Entries => InternalEntries;

        protected IHostsFileHandler FileHandler { get; }

        public bool HasUnsavedChanges { get; internal set; }

        public HostsUpdateResult Upsert(IPAddress address, string hostName, string comment = null)
        {
            Guard.IsNotNullOrWhiteSpace(hostName, nameof(hostName));
            var existing = InternalEntries.SingleOrDefault(entry => entry.HostName == hostName);
            if (existing != null)
            {
                if (existing.Address.Equals(address))
                {
                    return HostsUpdateResult.NoChange(existing);
                }
                InternalEntries.Remove(existing);
                InternalEntries.Add(new HostsEntry(address, hostName, comment));
                HasUnsavedChanges = true;
                return HostsUpdateResult.Updated(existing, address);
            }
            var addedEntry = new HostsEntry(address, hostName, comment);
            InternalEntries.Add(addedEntry);
            HasUnsavedChanges = true;
            return HostsUpdateResult.Added(addedEntry);
        }

        public void Load()
        {
            InternalEntries = FileHandler.Read().Select(line => new HostsEntry(line)).ToList();
            HasUnsavedChanges = false;
        }

        public void Save()
        {
            if (HasUnsavedChanges)
            {
                FileHandler.Write(InternalEntries.Select(entry=>entry.ToString()));
                HasUnsavedChanges = false;
            }
        }
    }
}