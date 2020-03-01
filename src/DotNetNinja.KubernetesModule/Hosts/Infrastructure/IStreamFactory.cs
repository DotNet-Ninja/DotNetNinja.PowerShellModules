using System.IO;

namespace DotNetNinja.KubernetesModule.Hosts.Infrastructure
{
    public interface IStreamFactory
    {
        Stream CreateWriteOnlyShared(string path);
        Stream CreateReadOnlyShared(string path);
    }
}