using System.IO;

namespace DotNetNinja.KubernetesModule.Hosts.Infrastructure
{
    public class FileStreamFactory : IStreamFactory
    {
        public Stream CreateWriteOnlyShared(string path) => new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);

        public Stream CreateReadOnlyShared(string path) => new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }
}