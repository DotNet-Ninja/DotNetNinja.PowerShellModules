using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DotNetNinja.KubernetesModule.Hosts.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class CommandShell : ICommandShell
    {
        public string Execute(string command, string arguments)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;

        }
    }
}