using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSAT.Models
{
    public interface IStaffRepository
    {
        Task<List<Staff>> GetStaff();
        Task<List<StaffJobDetails>> GetStaffDetails();
        Task<int> CreateStaffAsync(Staff staff);
        Task<int> CreateStaffJobDetailsAsync(StaffJobDetails staffDetails);
    }
}
