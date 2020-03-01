using Autofac;
using DotNetNinja.KubernetesModule.Hosts.Domain;
using DotNetNinja.KubernetesModule.Hosts.Infrastructure;

namespace DotNetNinja.KubernetesModule.Hosts
{
    public class ServiceLocator
    {
        public ServiceLocator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<HostsFile>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<KubernetesCluster>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<CommandShell>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<HostsFileHandler>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<FileStreamFactory>().AsImplementedInterfaces().InstancePerDependency();

            Container = builder.Build();
        }

        protected IContainer Container { get; }

        public TService Get<TService>() where TService : class
        {
            return Container.Resolve<TService>();
        }
    }
}