using AutoFixture;
using InterviewPanelAvailabilitySystemAPI.Controllers;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using InterviewPanelAvailabilitySystemAPI.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Controllers
{
    public class InterviewerControllerTests : IDisposable
    {
        private readonly Mock<IInterviewService> mockInterviewerService;

        public InterviewerControllerTests()
        {
            mockInterviewerService = new Mock<IInterviewService>();
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void AddInterviewer_ReturnsOk_WhenInterviewerIsAddedSuccessfully()
        {
            var fixture = new Fixture();
            var addInterviewerDto = fixture.Create<AddInterviewSlotsDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.AddInterviewSlot(It.IsAny<InterviewSlots>())).Returns(response);

            //Act

            var actual = target.AddInterviewSlot(addInterviewerDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockInterviewerService.Verify(c => c.AddInterviewSlot(It.IsAny<InterviewSlots>()), Times.Once);

        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void AddInterviewer_ReturnsBadRequest_WhenInterviewerIsNotAdded()
        {
            var fixture = new Fixture();
            var addInterviewerDto = fixture.Create<AddInterviewSlotsDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.AddInterviewSlot(It.IsAny<InterviewSlots>())).Returns(response);

            //Act

            var actual = target.AddInterviewSlot(addInterviewerDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockInterviewerService.Verify(c => c.AddInterviewSlot(It.IsAny<InterviewSlots>()), Times.Once);

        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void AddInterviewer_ReturnsBadRequest_ThrowsException()
        {
            var fixture = new Fixture();
            var addInterviewerDto = fixture.Create<AddInterviewSlotsDto>();
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.AddInterviewSlot(It.IsAny<InterviewSlots>())).Throws(new Exception("Simulated exception"));

            //Act

            var actual = target.AddInterviewSlot(addInterviewerDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal("Simulated exception", actual.Value);
            mockInterviewerService.Verify(c => c.AddInterviewSlot(It.IsAny<InterviewSlots>()), Times.Once);

        }
        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void DeleteInterviewSlot_ReturnsOkResponse_WhenInterviewerDeletedSuccessfully()
        {

            var interviewerId = 1;
            var slotDate = DateTime.Now;
            var timeSlotId = 1;

            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.RemoveSlot(interviewerId,slotDate,timeSlotId)).Returns(response);

            //Act

            var actual = target.DeleteInterviewSlot(interviewerId, slotDate, timeSlotId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockInterviewerService.Verify(c => c.RemoveSlot(interviewerId, slotDate, timeSlotId), Times.Once);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void DeleteInterviewSlot_ReturnsBadRequest_WhenInterviewerNotDeleted()
        {

            var interviewerId = 1;
            var slotDate = DateTime.Now;
            var timeSlotId = 1;
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.RemoveSlot(interviewerId, slotDate, timeSlotId)).Returns(response);

            //Act

            var actual = target.DeleteInterviewSlot(interviewerId, slotDate, timeSlotId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockInterviewerService.Verify(c => c.RemoveSlot(interviewerId, slotDate, timeSlotId), Times.Once);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void DeleteInterviewSlot_ReturnsBadRequest_ThrowsException()
        {
            int id = 1;
            var slotDate = DateTime.Now;
            var timeSlotId = 1;
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.RemoveSlot(id, slotDate, timeSlotId)).Throws(new Exception("Simulated exception"));

            //Act

            var actual = target.DeleteInterviewSlot(id, slotDate, timeSlotId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal("Simulated exception", actual.Value);
            mockInterviewerService.Verify(c => c.RemoveSlot(id, slotDate, timeSlotId), Times.Once);

        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void RemoveInterviewer_ReturnsBadRequest_WhenInterviewerIsLessThanZero()
        {

            var interviewerId = 0;
            var slotDate = DateTime.Now;
            var timeSlotId = 1;

            var target = new InterviewerController(mockInterviewerService.Object);

            //Act

            var actual = target.DeleteInterviewSlot(interviewerId, slotDate, timeSlotId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Enter correct data please", actual.Value);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void GetAllTimeslots_ReturnsOk_WhenTimeslotExists()
        {
            //Arrange
            var countries = new List<Timeslot>
            {
            new Timeslot{TimeslotId=1,TimeslotName="Timeslot 1"},
            new Timeslot{TimeslotId=2,TimeslotName="Timeslot 2"},
            };

            var response = new ServiceResponse<IEnumerable<TimeslotDto>>
            {
                Success = true,
                Data = countries.Select(c => new TimeslotDto { TimeslotId = c.TimeslotId, TimeslotName = c.TimeslotName }) // Convert to TimeslotDto
            };

            var mockTimeslotService = new Mock<IInterviewService>();
            var target = new InterviewerController(mockTimeslotService.Object);
            mockTimeslotService.Setup(c => c.GetAllTimeslots()).Returns(response);

            //Act
            var actual = target.GetAllTimeslots() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTimeslotService.Verify(c => c.GetAllTimeslots(), Times.Once);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void GetAllTimeslots_ReturnsNotFound_WhenNoTimeslotExists()
        {
            //Arrange


            var response = new ServiceResponse<IEnumerable<TimeslotDto>>
            {
                Success = false,
                Data = new List<TimeslotDto>()

            };

            var mockTimeslotService = new Mock<IInterviewService>();
            var target = new InterviewerController(mockTimeslotService.Object);
            mockTimeslotService.Setup(c => c.GetAllTimeslots()).Returns(response);

            //Act
            var actual = target.GetAllTimeslots() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);

            mockTimeslotService.Verify(c => c.GetAllTimeslots(), Times.Once);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void GetAllTimeslots_ReturnsBadRequest_ThrowsException()
        {
            //Arrange
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.GetAllTimeslots()).Throws(new Exception("Simulated exception"));

            //Act

            var actual = target.GetAllTimeslots() as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal("Simulated exception", actual.Value);
            mockInterviewerService.Verify(c => c.GetAllTimeslots(), Times.Once);

        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void GetAllInterviewslotsByEmployeeId_ReturnsOk_WhenTimeslotExists()
        {
            //Arrange
            var slots = new List<InterviewSlotsDto>
            {
            new InterviewSlotsDto{TimeslotId=1,SlotId=1},
            new InterviewSlotsDto{TimeslotId=2,SlotId=2},
            };


            var response = new ServiceResponse<IEnumerable<InterviewSlotsDto>>
            {
                Success = true,
                Data = slots.Select(c => new InterviewSlotsDto { TimeslotId = c.TimeslotId, SlotId = c.SlotId })
            };

            var mockTimeslotService = new Mock<IInterviewService>();
            var target = new InterviewerController(mockTimeslotService.Object);
            mockTimeslotService.Setup(c => c.GetAllInterviewslotsbyEmployeeId(1)).Returns(response);

            //Act
            var actual = target.GetAllInterviewslotsByEmployeeId(1) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockTimeslotService.Verify(c => c.GetAllInterviewslotsbyEmployeeId(1), Times.Once);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void GetAllInterviewslotsbyEmployeeId_ReturnsNotFound_WhenNoTimeslotExists()
        {
            //Arrange


            var response = new ServiceResponse<IEnumerable<InterviewSlotsDto>>
            {
                Success = false,
                Data = new List<InterviewSlotsDto>()

            };

            var mockTimeslotService = new Mock<IInterviewService>();
            var target = new InterviewerController(mockTimeslotService.Object);
            mockTimeslotService.Setup(c => c.GetAllInterviewslotsbyEmployeeId(1)).Returns(response);

            //Act
            var actual = target.GetAllInterviewslotsByEmployeeId(1) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);

            mockTimeslotService.Verify(c => c.GetAllInterviewslotsbyEmployeeId(1), Times.Once);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void GetAllInterviewslotsByEmployeeId_ReturnsBadRequest_ThrowsException()
        {
            //Arrange
            var target = new InterviewerController(mockInterviewerService.Object);
            mockInterviewerService.Setup(c => c.GetAllInterviewslotsbyEmployeeId(1)).Throws(new Exception("Simulated exception"));

            //Act

            var actual = target.GetAllInterviewslotsByEmployeeId(1) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal("Simulated exception", actual.Value);
            mockInterviewerService.Verify(c => c.GetAllInterviewslotsbyEmployeeId(1), Times.Once);

        }


        public void Dispose()
        {
            mockInterviewerService.VerifyAll();
        }
    }
}
