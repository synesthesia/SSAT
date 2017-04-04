using SSAT;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SSAT.Models;
using Moq;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Tests
{
    public class IntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public IntegrationTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        private async Task<string> GetResponseString()
        {
            var request = "/api/staff";

            var response = await _client.GetAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<HttpStatusCode> GetResponseCodePostAsync(StaffViewModel data)
        {
            var json = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("api/staff", content);
            response.EnsureSuccessStatusCode();

            return response.StatusCode;
        }

        [Fact]
        public async Task ReturnStaffListNotEmpty()
        {
            var responseString = await GetResponseString();

            Assert.NotEmpty(responseString);
        }

        [Fact]
        public async void PostNewStaffReturnsCreatedAsync()
        {
            StaffViewModel model = new StaffViewModel
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };

            var responseCode = await GetResponseCodePostAsync(model);

            Assert.Equal(HttpStatusCode.Created, responseCode);
        }
    }

    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IStaffRepository, StaffTestRepository>();

            services.AddSingleton(_config);

            services.AddLogging();
        }
    }
}
