using System;
using Xunit;
using SSAT.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using SSAT.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class UnitTests
    {
        [Fact]
        public async void ControllerPostCallsRepoAsync()
        {
            Mock<ILogger<StaffApiController>> logger = new Mock<ILogger<StaffApiController>>();
            Mock<IStaffRepository> repo = new Mock<IStaffRepository>();
            StaffApiController controller = new StaffApiController(repo.Object, logger.Object);

            StaffViewModel validStaffMember = new StaffViewModel
            {
                FirstName = "Test",
                LastName = "Test"
            };

            await controller.Post(validStaffMember);
            repo.Verify(r => r.CreateStaffAsync(It.IsAny<Staff>()));
        }

        [Fact]
        public async void InvalidPostHasCorrectResponseAsync()
        {
            var mockRepo = new Mock<IStaffRepository>();
            //mockRepo.Setup(repo => repo.GetStaff()).Returns(Task.FromResult(GetTestStaff()));

            var mockLogger = new Mock<ILogger<StaffApiController>>();
            var controller = new StaffApiController(mockRepo.Object, mockLogger.Object);
            controller.ModelState.AddModelError("LastName", "Required");
            var staff = new StaffViewModel { };
            var result = await controller.Post(staff);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
    }
}
