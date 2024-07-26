using AutoFixture;
using Fare;
using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Implementation;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Services
{
    public class RecruiterServiceTests : IDisposable
    {
        private readonly Mock<IRecruiterRepository> mockRepository;

        public RecruiterServiceTests()
        {
            mockRepository = new Mock<IRecruiterRepository>();
        }

        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void GetInterviewSlotsById_ReturnList_WhenNoContactExist()
        {
            //Arrange
            int slotId = 1;
            var target = new RecruiterService(mockRepository.Object);
            //Act
            var actual = target.GetInterviewSlotsById(slotId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("No records found.", actual.Message);
            Assert.False(actual.Success);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void GetInterviewSlotsById_ReturnsContactsList_WhenContactsExist()
        {
            //Arrange

            int slotId = 1;
            var contacts =
            new InterviewSlots { SlotId = 1, EmployeeId = 1, SlotDate = new DateTime(), TimeslotId = 1, IsBooked = true, Employee = new Employees { FirstName = "abc", InterviewRound = new InterviewRounds { InterviewRoundId = 1 }, JobRole = new JobRole { JobRoleId = 1 } }, Timeslot = new Timeslot { TimeslotId = 1 } };
            var response = new ServiceResponse<IEnumerable<InterviewSlotsDto>>
            {
                Success = true,
            };
            mockRepository.Setup(c => c.GetInterviewSlotsById(slotId)).Returns(contacts);
            var target = new RecruiterService(mockRepository.Object);

            //Act
            var actual = target.GetInterviewSlotsById(slotId);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockRepository.Verify(c => c.GetInterviewSlotsById(slotId), Times.Once);

        }
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void GetInterviewSlotsById_ReturnsException()
        {
            //Arrange

            int slotId = 1;
            mockRepository.Setup(c => c.GetInterviewSlotsById(slotId)).Throws(new Exception());
            var target = new RecruiterService(mockRepository.Object);

            //Act
            var actual = target.GetInterviewSlotsById(slotId);

            //Assert
            Assert.NotNull(actual);
            mockRepository.Verify(c => c.GetInterviewSlotsById(slotId), Times.Once);

        }

        //GetPaginatedInterviwerByAll
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void GetPaginatedInterviwerByAll_Should_Return_Correct_Response()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string searchQuery = "test";
            string sortOrder = "asc";
            int? jobRoleId = 1;
            int? roundId = null;

            var service = new RecruiterService(mockRepository.Object);

            var mockInterviewSlots = new List<InterviewSlots>
            {
                new InterviewSlots
                {
                    SlotId = 1,
                    EmployeeId = 1,
                    SlotDate = DateTime.UtcNow,
                    Employee = new Employees
                    {
                        FirstName = "test",
                        LastName = "test",
                        JobRoleId = 1,
                        Email = "test@example.com",
                        InterviewRoundId = 1,
                        IsActive = true,
                        IsAdmin = false,
                        IsRecruiter = true,
                        InterviewRound = new InterviewRounds
                        {
                            InterviewRoundId = 1,
                            InterviewRoundName = "First Round"
                        },
                        JobRole = new JobRole
                        {
                            JobRoleId = 1,
                            JobRoleName = "Developer"
                        }
                    },
                    TimeslotId = 1,
                    Timeslot = new Timeslot
                    {
                        TimeslotId = 1,
                        TimeslotName = "slot"
                    }
                }
            }.AsQueryable();
            mockRepository.Setup(r => r.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId))
                          .Returns(mockInterviewSlots);

            // Act
            var result = service.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
            mockRepository.Verify(r => r.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId), Times.Once);
        }


        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void GetPaginatedInterviwerByAll_Should_Return_NoRecordFound_When_NoInterviewSlots()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string searchQuery = "NonExistentName";
            string sortOrder = "asc";
            int? jobRoleId = 1;
            int? roundId = null;

           
            var service = new RecruiterService(mockRepository.Object);
            mockRepository.Setup(r => r.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId))
                          .Returns(Enumerable.Empty<InterviewSlots>().AsQueryable());

            // Act
            var result = service.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No record found", result.Message);
            Assert.Null(result.Data);
            mockRepository.Verify(r => r.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void GetPaginatedInterviwerByAll_ReturnException()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string searchQuery = "NonExistentName";
            string sortOrder = "asc";
            int? jobRoleId = 1;
            int? roundId = null;

            var service = new RecruiterService(mockRepository.Object);
            mockRepository.Setup(r => r.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId))
                         .Throws(new Exception());

            // Act
            var result = service.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            mockRepository.Verify(r => r.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId), Times.Once);
        }

        //TotalInterviewSlotsByAll
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void TotalInterviewSlotsByAll_Returns_Successful_Response()
        {
            // Arrange
            string searchQuery = "test";
            int? jobRoleId = 1;
            int? roundId = null;

            var service = new RecruiterService(mockRepository.Object);

            int expectedTotalInterviewSlots = 10;
            mockRepository.Setup(r => r.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId))
                          .Returns(expectedTotalInterviewSlots);

            // Act
            var result = service.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Pagination successful", result.Message);
            Assert.Equal(expectedTotalInterviewSlots, result.Data);
            mockRepository.Verify(r => r.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void TotalInterviewSlotsByAll_ReturnsException()
        {
            // Arrange
            string searchQuery = "test";
            int? jobRoleId = 1;
            int? roundId = null;

            var service = new RecruiterService(mockRepository.Object);

            int expectedTotalInterviewSlots = 0;
            mockRepository.Setup(r => r.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId))
                          .Throws(new Exception());

            // Act
            var result = service.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTotalInterviewSlots, result.Data);
            mockRepository.Verify(r => r.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId), Times.Once);
        }

        //ModifyInterviewSlots
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void ModifyInterviewSlots_Returns_InterviewSlotAlreadyBooked()
        {
            // Arrange
            var slots = new InterviewSlots
            {
                SlotId = 1,
                IsBooked = true
            };

           
            var service = new RecruiterService(mockRepository.Object);

            mockRepository.Setup(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked))
                          .Returns(true);

            // Act
            var result = service.ModifyInterviewSlots(slots);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Interview Slot Already Booked", result.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void ModifyInterviewSlots_Returns_InterviewSlotUpdatedSuccessfully()
        {
            // Arrange
            var slots = new InterviewSlots
            {
                SlotId = 1,
                IsBooked = false
            };
            var service = new RecruiterService(mockRepository.Object);

            var existingInterviewSlots = new InterviewSlots
            {
                SlotId = 1,
                IsBooked = false
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked))
                          .Returns(false);
            mockRepository.Setup(r => r.GetInterviewSlotsById(slots.SlotId))
                          .Returns(existingInterviewSlots);
            mockRepository.Setup(r => r.UpdateInterviewSlots(existingInterviewSlots))
                          .Returns(true);

            // Act
            var result = service.ModifyInterviewSlots(slots);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Interview slot booked successfully.", result.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked), Times.Once);
            mockRepository.Verify(r => r.GetInterviewSlotsById(slots.SlotId), Times.Once);
            mockRepository.Verify(r => r.UpdateInterviewSlots(existingInterviewSlots), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void ModifyInterviewSlots_ReturnsException()
        {
            // Arrange
            var slots = new InterviewSlots
            {
                SlotId = 1,
                IsBooked = false
            };
            var service = new RecruiterService(mockRepository.Object);

            var existingInterviewSlots = new InterviewSlots
            {
                SlotId = 1,
                IsBooked = false
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked))
                          .Returns(false);
            mockRepository.Setup(r => r.GetInterviewSlotsById(slots.SlotId))
                          .Returns(existingInterviewSlots);
            mockRepository.Setup(r => r.UpdateInterviewSlots(existingInterviewSlots))
                          .Throws(new Exception());

            // Act
            var result = service.ModifyInterviewSlots(slots);

            // Assert
            
            mockRepository.Verify(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked), Times.Once);
            mockRepository.Verify(r => r.GetInterviewSlotsById(slots.SlotId), Times.Once);
            mockRepository.Verify(r => r.UpdateInterviewSlots(existingInterviewSlots), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterServiceTests")]
        public void ModifyInterviewSlots_Returns_Error_Message_When_Update_Fails()
        {
            // Arrange
            var slots = new InterviewSlots
            {
                SlotId = 1,
                IsBooked = false
            };
            var service = new RecruiterService(mockRepository.Object);

            var existingInterviewSlots = new InterviewSlots
            {
                SlotId = 1,
                IsBooked = false
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked))
                          .Returns(false);
            mockRepository.Setup(r => r.GetInterviewSlotsById(slots.SlotId))
                          .Returns(existingInterviewSlots);
            mockRepository.Setup(r => r.UpdateInterviewSlots(existingInterviewSlots))
                          .Returns(false);

            // Act
            var result = service.ModifyInterviewSlots(slots);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after sometime.", result.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slots.SlotId, slots.IsBooked), Times.Once);
            mockRepository.Verify(r => r.GetInterviewSlotsById(slots.SlotId), Times.Once);
            mockRepository.Verify(r => r.UpdateInterviewSlots(existingInterviewSlots), Times.Once);
        }


        public void Dispose()
        {
            mockRepository.VerifyAll();
        }
    }
}
