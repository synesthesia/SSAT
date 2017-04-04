using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSAT.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SSAT.Controllers
{
    [Route("api/staff")]
    public class StaffApiController : Controller
    {
        private IStaffRepository _repository;
        private ILogger<StaffApiController> _logger;

        public StaffApiController(IStaffRepository repository, ILogger<StaffApiController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/staff
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var staff = await _repository.GetStaff();
                var staffDetails = await _repository.GetStaffDetails();

                var staffViewModels = (from s in staff
                                       join sd in staffDetails on s.ID equals sd.StaffID
                                       select new StaffViewModel
                                       {
                                           FirstName = s.FirstName,
                                           LastName = s.LastName,
                                           Age = s.Age,
                                           JobTitle = sd.JobTitle,
                                           SalaryGrade = sd.SalaryGrade
                                       }).ToList();

                return Ok(staffViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Staff: {ex}");

                return BadRequest(ex.Message);
            }
        }

        // POST api/staff
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StaffViewModel staff)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newStaffMember = new Staff { FirstName = staff.FirstName, LastName = staff.LastName, Age = staff.Age };
                    var createdStaffId = await _repository.CreateStaffAsync(newStaffMember);

                    var newStaffDetails = new StaffJobDetails { StaffID = createdStaffId, JobTitle = staff.JobTitle, StartDate = DateTime.Now, SalaryGrade = staff.SalaryGrade };
                    await _repository.CreateStaffJobDetailsAsync(newStaffDetails);

                    return Created($"api/staff/{staff.FullName}", staff);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Failed to get Staff: {ex}");

                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                }
            }

           return new BadRequestObjectResult(ModelState);
        }
    }
}
