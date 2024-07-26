using Fare;
using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Services
{
    public class ReportServicesTests : IDisposable
    {
        private readonly Mock<IReportRepository> mockRepository;

        public ReportServicesTests()
        {
            mockRepository = new Mock<IReportRepository>();
        }

        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void SlotsCountReport_Returns_WhenContactsExist()
        {

            // Arrange
            int jobRoleId = 1;
            var report = new ReportSlotCountDto()
            {
                AvailableSlot = 5,
                BookedSlot = 10
            };

            mockRepository.Setup(r => r.SlotsCountReport(jobRoleId, null,null,null)).Returns(report);

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.SlotsCountReport(jobRoleId, null, null, null);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(report, actual.Data);
            mockRepository.Verify(r => r.SlotsCountReport(jobRoleId, null, null, null), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void SlotsCountReport_ReturnsFalseSucessResponse_WhenSomethingWentWrong()
        {

            // Arrange
            int jobRoleId = 1;
            var report = new ReportSlotCountDto();
            report = null;

            mockRepository.Setup(r => r.SlotsCountReport(jobRoleId, null, null, null)).Returns(report);

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.SlotsCountReport(jobRoleId, null, null, null);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Something went wrong please try after sometime", actual.Message);
            mockRepository.Verify(r => r.SlotsCountReport(jobRoleId, null, null, null), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void SlotsCountReport_ThrowsException()
        {

            // Arrange
            int jobRoleId = 1;
            var mockRepository = new Mock<IReportRepository>();
            mockRepository.Setup(r => r.SlotsCountReport(jobRoleId, null, null, null)).Throws(new Exception("Simulated exception"));

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.SlotsCountReport(jobRoleId, null, null, null);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Simulated exception",actual.Message);
            mockRepository.Verify(r => r.SlotsCountReport(jobRoleId, null, null, null), Times.Once);
        }


        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void ReportDetail_ReturnsReport_WhenReportDataExist()
        {

            // Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 6;
            var interviewSlot = new List<InterviewSlots>
        {
            new InterviewSlots
            {
                EmployeeId = 1,
                TimeslotId = 1,
                SlotId = 1,
                IsBooked = true,
                SlotDate =DateTime.Now,
                Employee= new Employees
                {
                    EmployeeId = 1,
                    FirstName = "firstName",
                    LastName = "lastName",
                    Email = "user1@gmail.com"
                },
                Timeslot= new Timeslot
                {
                    TimeslotName="10:00AM-11:00AM"
                }

            },
            new InterviewSlots
            {
               EmployeeId = 2,
                TimeslotId = 2,
                SlotId = 2,
                IsBooked = false,
                SlotDate =DateTime.Now,
                Employee= new Employees
                {
                    EmployeeId = 2,
                    FirstName = "firstName",
                    LastName = "lastName",
                    Email = "user2@gmail.com"
                },
                Timeslot= new Timeslot
                {
                    TimeslotName="11:00AM-12:00PM"
                }
            }
        };

            mockRepository.Setup(r => r.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize)).Returns(interviewSlot);

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(interviewSlot.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void ReportDetail_ReturnsNoRecordFound()
        {

            // Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 6;
            var interviewSlot = new List<InterviewSlots>();

            mockRepository.Setup(r => r.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize)).Returns(interviewSlot);

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize), Times.Once);
        }   
        
        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void ReportDetail_ThrowsException()
        {

            // Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 6;
            var mockRepository = new Mock<IReportRepository>();
            mockRepository.Setup(r => r.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize)).Throws(new Exception("Simulated exception"));

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.ReportDetail(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Simulated exception", actual.Message);
            mockRepository.Verify(r => r.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void totalReportDetailCount_ReturnsCount()
        {

            // Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            var interviewSlot = new List<InterviewSlots>
        {
            new InterviewSlots
            {
                EmployeeId = 1,
                TimeslotId = 1,
                SlotId = 1,
                IsBooked = true,
                SlotDate =DateTime.Now,
                Employee= new Employees
                {
                    EmployeeId = 1,
                    FirstName = "firstName",
                    LastName = "lastName",
                    Email = "user1@gmail.com"
                },
                Timeslot= new Timeslot
                {
                    TimeslotName="10:00AM-11:00AM"
                }

            },
            new InterviewSlots
            {
               EmployeeId = 2,
                TimeslotId = 2,
                SlotId = 2,
                IsBooked = false,
                SlotDate =DateTime.Now,
                Employee= new Employees
                {
                    EmployeeId = 2,
                    FirstName = "firstName",
                    LastName = "lastName",
                    Email = "user2@gmail.com"
                },
                Timeslot= new Timeslot
                {
                    TimeslotName="11:00AM-12:00PM"
                }
            }
        };

            mockRepository.Setup(r => r.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked)).Returns(interviewSlot.Count);

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(interviewSlot.Count(), actual.Data);
            mockRepository.Verify(r => r.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked), Times.Once);
        }

        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void totalReportDetailCount_ReturnsSomethingWentWrong()
        {

            // Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            var interviewSlot = new List<InterviewSlots>();

            mockRepository.Setup(r => r.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked)).Returns(-1);

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal(interviewSlot.Count(), actual.Data);
            Assert.Equal("Something went wrong please try after sometime",actual.Message);
            mockRepository.Verify(r => r.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked), Times.Once);
        } 
        [Fact]
        [Trait("Report", "ReportServiceTests")]
        public void totalReportDetailCount_ThrowsException()
        {

            // Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            var interviewSlot = new List<InterviewSlots>();

            var mockRepository = new Mock<IReportRepository>();
            mockRepository.Setup(r => r.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked)).Throws(new Exception("Simulated exception"));

            var contactService = new ReportService(mockRepository.Object);

            // Act
            var actual = contactService.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal(0,actual.Data);
            Assert.Equal("Simulated exception",actual.Message);
            mockRepository.Verify(r => r.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked), Times.Once);
        }


        public void Dispose()
        {
            mockRepository.VerifyAll();
        }
    }

}

