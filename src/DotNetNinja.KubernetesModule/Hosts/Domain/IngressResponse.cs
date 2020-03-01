using System;

namespace DotNetNinja.KubernetesModule.Hosts.Domain
{
    public class IngressResponse
    {
        private static readonly char[] Delimiters = new[] {'\t', ' '};
        public IngressResponse(string kubectlLine)
        {
            Raw = kubectlLine??string.Empty;
            var fields = Raw.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length == 6)
            {
                Namespace = fields[0];
                Name = fields[1];
                Hosts = fields[2];
                Address = fields[3];
                Ports = fields[4];
                Age = fields[5];
                IsValid = true;
            }
        }

        public string Raw { get; }
        public string Namespace { get; }
        public string Name { get; }
        public string Hosts { get; }
        public string Address { get; }
        public string Ports { get; }
        public string Age { get; }
        public bool IsValid { get; }
        public bool IsHeader => 
            Namespace == "NAMESPACE" 
            && Name == "NAME" 
            && Hosts == "HOSTS" 
            && Address == "ADDRESS" 
            && Ports == "PORTS" 
            && Age == "AGE";

        public override string ToString() => Raw;
    }
}