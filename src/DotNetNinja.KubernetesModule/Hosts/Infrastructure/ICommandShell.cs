namespace DotNetNinja.KubernetesModule.Hosts.Infrastructure
{
    public interface ICommandShell
    {
        string Execute(string command, string arguments);
    }
}