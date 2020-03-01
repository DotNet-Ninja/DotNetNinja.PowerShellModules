using System;
using System.Collections.Generic;
using System.IO;
using ChaosMonkey.Guards;

namespace DotNetNinja.KubernetesModule.Hosts.Infrastructure
{
    public class HostsFileHandler : IHostsFileHandler
    {
        public static readonly string HostsPathEnvironmentVariable = Environment.GetEnvironmentVariable("HostsPath");

        public static readonly string HostsPath = HostsPathEnvironmentVariable 
                      ?? $"{Environment.GetFolderPath(Environment.SpecialFolder.System)}\\drivers\\etc\\hosts";

        public HostsFileHandler(IStreamFactory factory)
        {
            Guard.IsNotNull(factory, nameof(factory));
            StreamFactory = factory;
        }

        protected IStreamFactory StreamFactory { get; }

        public List<string> Read()
        {
            var result = new List<string>();
            using (var stream = StreamFactory.CreateReadOnlyShared(HostsPath))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine());
                }
            }
            return result;
        }

        public void Write(IEnumerable<string> lines)
        {
            using (var stream = StreamFactory.CreateWriteOnlyShared(HostsPath))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}