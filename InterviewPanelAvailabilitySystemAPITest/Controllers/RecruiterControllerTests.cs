using AutoFixture;
using InterviewPanelAvailabilitySystemAPI.Controllers;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using InterviewPanelAvailabilitySystemAPI.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InterviewPanelAvailabilitySystemAPITest.Controllers
{
    public class RecruiterControllerTests : IDisposable
    {
        private readonly Mock<IRecruiterService> mockService;

        public RecruiterControllerTests()
        {
            mockService = new Mock<IRecruiterService>();
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetInterviewSlotsById_ReturnsOkWithContacts_WhenResponseIsSuccess()
        {
            //Arrange
            var contacts = new InterviewSlotsDto { EmployeeId = 1, SlotId = 1 };

            int slotId = 1;
            var response = new ServiceResponse<InterviewSlotsDto>
            {
                Data = contacts,
                Success = true,
            };


            var target = new RecruiterController(mockService.Object);
            mockService.Setup(c => c.GetInterviewSlotsById(slotId)).Returns(response);

            //Act
            var actual = target.GetInterviewSlotsById(slotId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockService.Verify(c => c.GetInterviewSlotsById(slotId), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetInterviewSlotsById_ReturnsNotFound_WhenResponseIsFalse()
        {
            //Arrange

            int slotId = 1;
            var response = new ServiceResponse<InterviewSlotsDto>
            {
                Success = false,
            };


            var target = new RecruiterController(mockService.Object);
            mockService.Setup(c => c.GetInterviewSlotsById(slotId)).Returns(response);

            //Act
            var actual = target.GetInterviewSlotsById(slotId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockService.Verify(c => c.GetInterviewSlotsById(slotId), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetInterviewSlotsById_ReturnsException()
        {
            //Arrange

            int slotId = 1;
            var response = new ServiceResponse<InterviewSlotsDto>
            {
                Success = false,
            };


            var target = new RecruiterController(mockService.Object);
            mockService.Setup(c => c.GetInterviewSlotsById(slotId)).Throws(new Exception());

            //Act
            var actual = target.GetInterviewSlotsById(slotId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Value);
            mockService.Verify(c => c.GetInterviewSlotsById(slotId), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void ModifyInterviewSlots_ReturnsOk_WhenUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateInterviewSlotsDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var target = new RecruiterController(mockService.Object);
            mockService.Setup(c => c.ModifyInterviewSlots(It.IsAny<InterviewSlots>())).Returns(response);

            //Act

            var actual = target.UpdateInterviewSlot(updateContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockService.Verify(c => c.ModifyInterviewSlots(It.IsAny<InterviewSlots>()), Times.Once);

        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void ModifyInterviewSlots_ReturnsBadRequesr_WhenNotUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateInterviewSlotsDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var target = new RecruiterController(mockService.Object);
            mockService.Setup(c => c.ModifyInterviewSlots(It.IsAny<InterviewSlots>())).Returns(response);

            //Act

            var actual = target.UpdateInterviewSlot(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockService.Verify(c => c.ModifyInterviewSlots(It.IsAny<InterviewSlots>()), Times.Once);

        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void ModifyInterviewSlots_ReturnException()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateInterviewSlotsDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var target = new RecruiterController(mockService.Object);
            mockService.Setup(c => c.ModifyInterviewSlots(It.IsAny<InterviewSlots>())).Throws(new Exception());

            //Act

            var actual = target.UpdateInterviewSlot(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            mockService.Verify(c => c.ModifyInterviewSlots(It.IsAny<InterviewSlots>()), Times.Once);

        }

        //GetPaginatedInterviwerByAll
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetPaginatedInterviwerByAll_Returns_OkResult()
        {
            // Arrange
            int page = 1;
            int pageSize = 20;
            string searchQuery = "";
            string sortOrder = "asc";
            int? jobRoleId = null;
            int? roundId = null;
            var controller = new RecruiterController(mockService.Object);

            var mockInterviewers = new List<InterviewSlotsDto>
            {
                new InterviewSlotsDto { SlotId = 1 },
                new InterviewSlotsDto { SlotId = 2 }
            };

            var serviceResponse = new ServiceResponse<IEnumerable<InterviewSlotsDto>>
            {
                Success = true,
                Data = mockInterviewers
            };

            mockService
                .Setup(x => x.GetPaginatedInterviwerByAll(
                    page,
                    pageSize,
                    searchQuery,
                    sortOrder,
                    jobRoleId,
                    roundId))
                .Returns(serviceResponse);

            // Act
            var result = controller.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsAssignableFrom<ServiceResponse<IEnumerable<InterviewSlotsDto>>>(okResult.Value);
            Assert.True(responseData.Success);
            Assert.Equal(mockInterviewers.Count, responseData.Data.Count());
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetPaginatedInterviwerByAll_Returns_NotFoundResult_When_Response_Fails()
        {
            // Arrange
            int page = 1;
            int pageSize = 20;
            string searchQuery = "";
            string sortOrder = "asc";
            int? jobRoleId = null;
            int? roundId = null;

            var controller = new RecruiterController(mockService.Object);

            var serviceResponse = new ServiceResponse<IEnumerable<InterviewSlotsDto>>
            {
                Success = false,
                Message = "Failed to fetch interviewers"
            };

            mockService
                .Setup(x => x.GetPaginatedInterviwerByAll(
                    page,
                    pageSize,
                    searchQuery,
                    sortOrder,
                    jobRoleId,
                    roundId))
                .Returns(serviceResponse);

            // Act
            var result = controller.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseData = Assert.IsType<ServiceResponse<IEnumerable<InterviewSlotsDto>>>(notFoundResult.Value);
            Assert.False(responseData.Success);
            Assert.Equal("Failed to fetch interviewers", responseData.Message);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetPaginatedInterviwerByAll_ReturnsException()
        {
            // Arrange
            int page = 1;
            int pageSize = 20;
            string searchQuery = "";
            string sortOrder = "asc";
            int? jobRoleId = null;
            int? roundId = null;

            var controller = new RecruiterController(mockService.Object);

            var serviceResponse = new ServiceResponse<IEnumerable<InterviewSlotsDto>>
            {
                Success = false,
                Message = "Failed to fetch interviewers"
            };

            mockService
                .Setup(x => x.GetPaginatedInterviwerByAll(
                    page,
                    pageSize,
                    searchQuery,
                    sortOrder,
                    jobRoleId,
                    roundId))
               .Throws(new Exception());

            // Act
            var result = controller.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

            // Assert
            var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
          
        }


        //GetTotalInterviewSlotsByAll
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetTotalInterviewSlotsByAll_Returns_OkObjectResult_When_Successful()
        {
            // Arrange
            var searchQuery = "query";
            int? jobRoleId = 1;
            int? roundId = 2;

            var controller = new RecruiterController(mockService.Object);

            var mockInterviewSlotsCount = 10; 

            var serviceResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = mockInterviewSlotsCount
            };

            mockService
                .Setup(s => s.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId))
                .Returns(serviceResponse);

            // Act
            var result = controller.GetTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var responseData = Assert.IsAssignableFrom<ServiceResponse<int>>(result.Value);
            Assert.True(responseData.Success);
            Assert.Equal(mockInterviewSlotsCount, responseData.Data);
            mockService.Verify(s => s.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetTotalInterviewSlotsByAll_ReturnsException()
        {
            // Arrange
            var searchQuery = "query";
            int? jobRoleId = 1;
            int? roundId = 2;

            var controller = new RecruiterController(mockService.Object);

            var mockInterviewSlotsCount = 10;

            var serviceResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = mockInterviewSlotsCount
            };

            mockService
                .Setup(s => s.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId))
                .Throws(new Exception());

            // Act
            var result = controller.GetTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            mockService.Verify(s => s.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void GetTotalInterviewSlotsByAll_Returns_NotFoundObjectResult_When_NotSuccessful()
        {
            // Arrange
            var searchQuery = "query";
            int? jobRoleId = 1;
            int? roundId = 2;

            var controller = new RecruiterController(mockService.Object);

            var serviceResponse = new ServiceResponse<int>
            {
                Success = false,
                Message = "Interview slots not found."
            };

            mockService
                .Setup(s => s.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId))
                .Returns(serviceResponse);

            // Act
            var result = controller.GetTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            var responseData = Assert.IsAssignableFrom<ServiceResponse<int>>(result.Value);
            Assert.False(responseData.Success);
            Assert.Equal(serviceResponse.Message, responseData.Message);
            mockService.Verify(s => s.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId), Times.Once);
        }
        public void Dispose()
        {
            mockService.VerifyAll();
        }
    }
}
