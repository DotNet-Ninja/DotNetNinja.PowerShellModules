using System;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using ChaosMonkey.Guards;
using DotNetNinja.KubernetesModule.Hosts.Constants;
using DotNetNinja.KubernetesModule.Hosts.Domain;

namespace DotNetNinja.KubernetesModule.Hosts
{

    [Cmdlet(VerbsData.Update, Nouns.HostsForIngress)]
    [OutputType(typeof(HostsUpdateResult))]
    public class UpdateHostsForIngressCmdLet: Cmdlet
    {
        public UpdateHostsForIngressCmdLet()
        {
            var services = new ServiceLocator();
            Hosts = services.Get<IHostsFile>();
            Cluster = services.Get<IKubernetesCluster>();
        }

        public UpdateHostsForIngressCmdLet(IHostsFile hosts, IKubernetesCluster cluster)
        {
            Guard.IsNotNull(hosts, nameof(hosts));
            Guard.IsNotNull(cluster, nameof(cluster));
            Hosts = hosts;
            Cluster = cluster;
        }
        
        protected internal IHostsFile Hosts { get; private set; }
        protected internal  IKubernetesCluster Cluster { get; private set; }
        
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true)]
        public string[] HostNames { get; set; }

        protected override void BeginProcessing()
        {
            Hosts.Load();
        }

        protected override void ProcessRecord()
        {
            try
            {
                var responses = Cluster.GetIngresses().ToList();

                if (responses.Any())
                {
                    var ingresses = responses.GetValidIngresses().ToList();
                    if (HostNames != null && HostNames.Any())
                    {
                        ingresses = ingresses.Where(ingress => HostNames.Contains(ingress.Hosts)).ToList();
                    }
                    
                    var results = ingresses.Select(ingress =>
                            Hosts.Upsert(ingress.Address, ingress.Hosts, $" Name: {ingress.Name} Namespace: {ingress.Namespace} EntryDate:{DateTime.Now}"))
                        .OrderBy(result => result.Status)
                        .ThenBy(result => result.HostName);
                    foreach (var result in results)
                    {
                        WriteObject(result);
                    }
                    return;
                }
                WriteWarning("No response was received from kubernetes.  Is your cluster running? (If running minikube locally, try 'minikube status' and/or 'minikube start')");
            }
            catch (Win32Exception ex)
            {
                if (ex.Message.Contains("The system cannot find the file specified."))
                {
                    WriteWarning("The kubectl command was not found on your system. (Is it installed and on your path?)");
                    WriteError(new ErrorRecord(ex, Errors.KubectlNotFound, ErrorCategory.ResourceUnavailable, Cluster));
                    return;
                }
                throw;
            }
        }

        protected override void EndProcessing()
        {
            Hosts.Save();
        }

        protected override void StopProcessing()
        {
            Cluster = null;
            Hosts = null;
        }
    }
}
