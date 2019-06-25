using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Shouldly;
using Xunit;

namespace Microsoft.Extensions.Configuration.Consul.Tests
{
    public class IntegrationTests : IDisposable
    {
        private readonly ConsulClient _client;
        private readonly string _prefix;
        private readonly ConsulConfigurationSource _source;
        public IntegrationTests()
        {
            _client = new ConsulClient(c => c.Address = new Uri("http://k8s.yx.com:28500"));
            _prefix = "appsettings/twelve/";
            _source = new ConsulConfigurationSource(_prefix,
                QueryOptions.Default,
                () => new ConsulClient(c => c.Address = new Uri("http://k8s.yx.com:28500")));
        }

        [RequiresConsulFact]
        public void When_reading_from_non_existing_store()
        {
            var config = new ConfigurationBuilder()
                .AddConsul(_source)
                .Build()
                .Get<Configuration>();

            config.ShouldBeNull();
        }

        [Fact]
        public async void When_reading_values_which_exist()
        {
            await Write(nameof(Configuration.Name), "the name");
            await Write(nameof(Configuration.Description), "the description");

            var config = new ConfigurationBuilder()
                .AddConsul(_source)
                .Build()
                .Get<Configuration>();

            config.ShouldSatisfyAllConditions(
                () => config.Name.ShouldBe("the name"),
                () => config.Description.ShouldBe("the description")
            );
        }

        private Task Write(string key, string value) => _client.KV.Put(Pair(key, value));
        private KVPair Pair(string key, string value) => new KVPair(_prefix + key) { Value = Encoding.UTF8.GetBytes(value) };

        public void Dispose()
        {
            //_client.KV.DeleteTree(_prefix).Wait();
            _client.Dispose();
        }


        public class Configuration
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
