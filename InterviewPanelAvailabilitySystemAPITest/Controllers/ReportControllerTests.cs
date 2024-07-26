using AutoFixture;
using InterviewPanelAvailabilitySystemAPI.Controllers;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Controllers
{
    public class ReportControllerTests : IDisposable
    {
        private readonly Mock<IReportService> mockReporttService;

        public ReportControllerTests()
        {
            mockReporttService = new Mock<IReportService>();
        }
        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void SlotsCountReport_ReturnsOkWithContacts()
        {
            //Arrange
            var successResponse = new ReportSlotCountDto
            {
                AvailableSlot = 10,
                BookedSlot = 5
            };

            int jobRoleId = 1;
            var response = new ServiceResponse<ReportSlotCountDto>
            {
                Success = true,
                Data = successResponse
            };

            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.SlotsCountReport(jobRoleId, null, null, null)).Returns(response);

            //Act
            var actual = target.SlotsCountReport(jobRoleId, null, null, null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockReporttService.Verify(c => c.SlotsCountReport(jobRoleId, null, null, null), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void SlotsCountReport_ReturnsNotFound()
        {
            //Arrange
            var successResponse = new ReportSlotCountDto();
            successResponse = null;

            int jobRoleId = 1;
            var response = new ServiceResponse<ReportSlotCountDto>
            {
                Success = false,
                Data = successResponse,
                Message = "Something went wrong please try after sometime"
            };

            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.SlotsCountReport(jobRoleId, null, null, null)).Returns(response);

            //Act
            var actual = target.SlotsCountReport(jobRoleId, null, null, null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockReporttService.Verify(c => c.SlotsCountReport(jobRoleId, null, null, null), Times.Once);
        }


        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void SlotsCountReport_ThrowsException()
        {
            //Arrange
            int jobRoleId = 1;
            var mockReporttService = new Mock<IReportService>();
            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.SlotsCountReport(jobRoleId, null, null, null)).Throws(new Exception("Simulated exception"));

            //Act
            var actual = target.SlotsCountReport(jobRoleId, null, null, null) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal("Simulated exception", actual.Value);
            mockReporttService.Verify(c => c.SlotsCountReport(jobRoleId, null, null, null), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void ReportDetails_ReturnsOkWithReportDetails()
        {
            //Arrange
            var fixture = new Fixture();
            var report = fixture.Create<IEnumerable<ReportDetailsDto>>();

            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 6;

            var response = new ServiceResponse<IEnumerable<ReportDetailsDto>>
            {
                Success = true,
                Data = report.Select(r => new ReportDetailsDto { EmployeeId = r.EmployeeId, FirstName = r.FirstName, LastName = r.LastName, Email = r.Email, SlotDate = r.SlotDate, TimeslotId = r.TimeslotId, timeSlotName = r.timeSlotName })
            };

            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize)).Returns(response);

            //Act
            var actual = target.ReportDetails(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockReporttService.Verify(c => c.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void ReportDetails_ReturnsNotFound()
        {
            //Arrange

            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 6;

            var response = new ServiceResponse<IEnumerable<ReportDetailsDto>>
            {
                Success = false,
                Data = null,
                Message = "Something went wrong please try after sometime"
            };

            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize)).Returns(response);

            //Act
            var actual = target.ReportDetails(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockReporttService.Verify(c => c.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize), Times.Once);
        }
        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void ReportDetails_ThrowsException()
        {
            //Arrange

            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 6;

            var mockReporttService = new Mock<IReportService>();
            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize)).Throws(new Exception("Simulated exception"));

            //Act
            var actual = target.ReportDetails(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal("Simulated exception", actual.Value);
            mockReporttService.Verify(c => c.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void TotalReportDetailCount_ReturnsOkWithCount()
        {
            //Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;

            var interviewSlots = new List<InterviewSlots>
            {
                new InterviewSlots{EmployeeId=1,SlotId=1, TimeslotId = 1},
                new InterviewSlots{EmployeeId=2,SlotId=2, TimeslotId = 5},
            };

            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = interviewSlots.Count()
            };

            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked)).Returns(response);

            //Act
            var actual = target.TotalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockReporttService.Verify(c => c.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void TotalReportDetailCount_ReturnsNotFound()
        {
            //Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;

            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked)).Returns(response);

            //Act
            var actual = target.TotalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockReporttService.Verify(c => c.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportControllerTests")]
        public void TotalReportDetailCount_ThrowsException()
        {
            //Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;

            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockReporttService = new Mock<IReportService>();
            var target = new ReportController(mockReporttService.Object);
            mockReporttService.Setup(c => c.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked)).Throws(new Exception("Simulated exception"));

            //Act
            var actual = target.TotalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal("Simulated exception", actual.Value);
            Assert.Equal(0, response.Data);
            mockReporttService.Verify(c => c.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked), Times.Once);
        }


        public void Dispose()
        {
            mockReporttService.VerifyAll();
        }
    }
}
