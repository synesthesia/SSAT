using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SSAT.Models
{
    public class StaffRemoteRepository : IStaffRepository
    {
        private IConfigurationRoot _config;
        private ILogger<StaffRemoteRepository> _logger;
        static HttpClient _client = new HttpClient();

        public StaffRemoteRepository(IConfigurationRoot config, ILogger<StaffRemoteRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<List<Staff>> GetStaff()
        {
            var retValue = new List<Staff>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config["Resources:ExternalResourceUri"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/staff");
                response.EnsureSuccessStatusCode();

                retValue = await response.Content.ReadAsStringAsync().ContinueWith<List<Staff>>(postTask =>
                {
                    return JsonConvert.DeserializeObject<List<Staff>>(postTask.Result);
                });
            }

            return retValue;
        }

        public async Task<List<StaffJobDetails>> GetStaffDetails()
        {
            var retValue = new List<StaffJobDetails>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config["Resources:ExternalResourceUri"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/staffDetails");
                response.EnsureSuccessStatusCode();

                retValue = await response.Content.ReadAsStringAsync().ContinueWith<List<StaffJobDetails>>(postTask =>
                {
                    return JsonConvert.DeserializeObject<List<StaffJobDetails>>(postTask.Result);
                });

            }

            return retValue;
        }

        public async Task<int> CreateStaffAsync(Staff staff)
        {
            var output = JsonConvert.SerializeObject(staff);
            HttpContent content = new StringContent(output);

            HttpResponseMessage response = await _client.PostAsync("api/staff", content);
            response.EnsureSuccessStatusCode();

            var staffCreated = await response.Content.ReadAsStringAsync().ContinueWith<Staff>(postTask =>
            {
                return JsonConvert.DeserializeObject<Staff>(postTask.Result);
            });

            _logger.LogInformation($"Staff Member Created: {staff.ID}");

            return staff.ID;
        }

        public async Task<int> CreateStaffJobDetailsAsync(StaffJobDetails staffDetails)
        {
            var output = JsonConvert.SerializeObject(staffDetails);
            HttpContent content = new StringContent(output);

            HttpResponseMessage response = await _client.PostAsync("api/staffJobDetails", content);
            response.EnsureSuccessStatusCode();

            var staffJobCreated = await response.Content.ReadAsStringAsync().ContinueWith<StaffJobDetails>(postTask =>
            {
                return JsonConvert.DeserializeObject<StaffJobDetails>(postTask.Result);
            });

            _logger.LogInformation($"Staff Job Details Created: {staffJobCreated.ID}");

            return staffJobCreated.ID;
        }
    }
}
