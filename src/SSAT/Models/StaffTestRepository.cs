using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SSAT.Models
{
    public class StaffTestRepository : IStaffRepository
    {
        private ILogger<StaffTestRepository> _logger;

        public StaffTestRepository(ILogger<StaffTestRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<Staff>> GetStaff()
        {
            Task<List<Staff>> task = Task<List<Staff>>.Factory.StartNew(
                () => {
                    return new List<Staff> {
                    new Staff { ID = 0, FirstName = "Sebastian",LastName = "Smith", Age = 32 },
                    new Staff { ID = 1, FirstName = "FirstName",LastName="LastName", Age = 100 },
                    new Staff { ID = 2, FirstName = "FirstName2", LastName = "LastName2", Age = 100 }};
            });

            return await task;
        }

        public async Task<List<StaffJobDetails>> GetStaffDetails()
        {
            Task<List<StaffJobDetails>> task = Task<List<StaffJobDetails>>.Factory.StartNew(
               () => {
                   return new List<StaffJobDetails> {
                    new StaffJobDetails { ID = 0, StaffID = 0, JobTitle = "Title 1" },
                    new StaffJobDetails { ID = 1,  StaffID = 1, JobTitle = "Title 2" },
                    new StaffJobDetails { ID = 2,  StaffID = 2, JobTitle = "Title 3" }};
               });

            return await task;
        }

        async Task<int> IStaffRepository.CreateStaffAsync(Staff staff)
        {
            var task = Task<int>.Factory.StartNew((() => { return 0; }));
            return await task;
        }

        async Task<int> IStaffRepository.CreateStaffJobDetailsAsync(StaffJobDetails staffDetails)
        {
            var task = Task<int>.Factory.StartNew((() => { return 0; }));
            return await task;
        }
    }
}
