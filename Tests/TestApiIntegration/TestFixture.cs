// Integration Tests - https://www.codingame.com/playgrounds/35462/creating-web-api-in-asp-net-core-2-0/part-3---integration-tests

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ApiIntegrationTest
{
    public class TestFixture : IDisposable
    {
        private static IConfigurationRoot Config => new ConfigurationBuilder().AddJsonFile("config.json").AddEnvironmentVariables().Build();

        public HttpClient Client { get; }
        public Newtonsoft.Json.JsonSerializerSettings SerializerOptions { get; }
        public TestFixture()
        {
            Client = new HttpClient
            {
                // BaseAddress = new Uri("http://localhost:55673/")
                BaseAddress = new Uri(Config["api_url"])
            };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SerializerOptions = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}