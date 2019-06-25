using System;
using Consul;

namespace Microsoft.Extensions.Configuration.Consul
{
    public class ConsulConfigurationSource : IConfigurationSource
    {
        public Func<IConsulClient> ClientFactory { get; set; }
        public string Prefix { get; set; }
        public QueryOptions Options { get; set; }

        public ConsulConfigurationSource(): 
            this(string.Empty, QueryOptions.Default,  () => new ConsulClient())
        {
        }

        public ConsulConfigurationSource(string prefix, QueryOptions options, Func<IConsulClient> clientFactory)
        {
            ClientFactory = clientFactory;
            Prefix = prefix;
            Options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(ClientFactory, Options, Prefix);
        }
    }
}
