using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
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
    public class InterviewerServiceTests : IDisposable
    {
        private readonly Mock<IInterviewRepository> mockRepository;

        public InterviewerServiceTests()
        {
            mockRepository = new Mock<IInterviewRepository>();
        }
        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void AddInterviewSlot_ReturnsSlotSavedSuccessfully_WhenSaved()
        {
            var slot = new InterviewSlots()
            {
                SlotId = 1,
                EmployeeId = 1,
                SlotDate = DateTime.Now.Date.AddDays(1),
                TimeslotId = 1,
                IsBooked = false,
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slot.EmployeeId,slot.SlotDate,slot.TimeslotId)).Returns(false);
            mockRepository.Setup(r => r.AddInterviewSlot(slot)).Returns(true);


            var contactService = new InterviewService(mockRepository.Object);

            // Act
            var actual = contactService.AddInterviewSlot(slot);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Slot Saved Successfully", actual.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slot.EmployeeId, slot.SlotDate, slot.TimeslotId), Times.Once);
            mockRepository.Verify(r => r.AddInterviewSlot(slot), Times.Once);


        }
        
        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void AddInterviewSlot_ReturnsSomethingWentwrong_WhenNotSaved()
        {
            var slot = new InterviewSlots()
            {
                SlotId = 1,
                EmployeeId = 1,
                SlotDate = DateTime.Now.Date.AddDays(1),
                TimeslotId = 1,
                IsBooked = false,
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slot.EmployeeId,slot.SlotDate,slot.TimeslotId)).Returns(false);
            mockRepository.Setup(r => r.AddInterviewSlot(slot)).Returns(false);


            var contactService = new InterviewService(mockRepository.Object);

            // Act
            var actual = contactService.AddInterviewSlot(slot);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong. Please try later", actual.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slot.EmployeeId, slot.SlotDate, slot.TimeslotId), Times.Once);
            mockRepository.Verify(r => r.AddInterviewSlot(slot), Times.Once);


        } 
        
        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void AddInterviewSlot_ReturnsAlreadyExist()
        {
            var slot = new InterviewSlots()
            {
                SlotId = 1,
                EmployeeId = 1,
                SlotDate = DateTime.Now.Date.AddDays(1),
                TimeslotId = 1,
                IsBooked = false,
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slot.EmployeeId,slot.SlotDate,slot.TimeslotId)).Returns(true);

            var contactService = new InterviewService(mockRepository.Object);

            // Act
            var actual = contactService.AddInterviewSlot(slot);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Interview Slot Already Booked", actual.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slot.EmployeeId, slot.SlotDate, slot.TimeslotId), Times.Once);


        }
          [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void AddInterviewSlot_ReturnsError_DateIsPast10Days()
        {
            var slot = new InterviewSlots()
            {
                SlotId = 1,
                EmployeeId = 1,
                SlotDate = DateTime.Now.Date.AddDays(12),
                TimeslotId = 1,
                IsBooked = false,
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slot.EmployeeId,slot.SlotDate,slot.TimeslotId)).Returns(false);

            var contactService = new InterviewService(mockRepository.Object);

            // Act
            var actual = contactService.AddInterviewSlot(slot);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Can't book slots for after 10 days", actual.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slot.EmployeeId, slot.SlotDate, slot.TimeslotId), Times.Once);


        }
          [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void AddInterviewSlot_ReturnsError_DateIsInPast()
        {
            var slot = new InterviewSlots()
            {
                SlotId = 1,
                EmployeeId = 1,
                SlotDate = DateTime.Now.Date,
                TimeslotId = 1,
                IsBooked = false,
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slot.EmployeeId,slot.SlotDate,slot.TimeslotId)).Returns(false);

            var contactService = new InterviewService(mockRepository.Object);

            // Act
            var actual = contactService.AddInterviewSlot(slot);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Can't book slots for past date", actual.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slot.EmployeeId, slot.SlotDate, slot.TimeslotId), Times.Once);


        }

        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void AddInterviewSlot_ThrowsException()
        {
            var slot = new InterviewSlots()
            {
                SlotId = 1,
                EmployeeId = 1,
                SlotDate = DateTime.Now.Date.AddDays(1),
                TimeslotId = 1,
                IsBooked = false,
            };

            mockRepository.Setup(r => r.InterviewSlotExist(slot.EmployeeId, slot.SlotDate, slot.TimeslotId)).Returns(false);
            mockRepository.Setup(r => r.AddInterviewSlot(slot)).Throws(new Exception("Simulated exception")); 


            var contactService = new InterviewService(mockRepository.Object);

            // Act
            var actual = contactService.AddInterviewSlot(slot);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Simulated exception", actual.Message);
            mockRepository.Verify(r => r.InterviewSlotExist(slot.EmployeeId, slot.SlotDate, slot.TimeslotId), Times.Once);
            mockRepository.Verify(r => r.AddInterviewSlot(slot), Times.Once);


        }

        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void RemoveInterviewSlot_ReturnsDeletedSuccessfully_WhenDeletedSuccessfully()
        {
            var id = 1;
            var slotDate = DateTime.Now.Date.AddDays(1);
            var timeSlotId = 1;
            mockRepository.Setup(r => r.DeleteInterviewSlot(id,slotDate,timeSlotId)).Returns(true);

            var interviewService = new InterviewService(mockRepository.Object);

            // Act
            var actual = interviewService.RemoveSlot(id,slotDate,timeSlotId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Slot deleted successfully", actual.Message);
            mockRepository.Verify(r => r.DeleteInterviewSlot(id, slotDate, timeSlotId), Times.Once);
        }

        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void RemoveInterviewer_SomethingWentWrong_WhenDeletionFailed()
        {

            var id = 1;
            var slotDate = DateTime.Now.Date.AddDays(1);
            var timeSlotId = 1;
            mockRepository.Setup(r => r.DeleteInterviewSlot(id, slotDate, timeSlotId)).Returns(false);

            var interviewService = new InterviewService(mockRepository.Object);

            // Act
            var actual = interviewService.RemoveSlot(id, slotDate, timeSlotId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong", actual.Message);
            mockRepository.Verify(r => r.DeleteInterviewSlot(id, slotDate, timeSlotId), Times.Once);
        }      
        
        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void RemoveInterviewslot_ThrowsException()
        {

            var id = 1;
            var slotDate = DateTime.Now.Date.AddDays(1);
            var timeSlotId = 1;
            mockRepository.Setup(r => r.DeleteInterviewSlot(id, slotDate, timeSlotId)).Throws(new Exception("Simulated exception"));

            var interviewService = new InterviewService(mockRepository.Object);

            // Act
            var actual = interviewService.RemoveSlot(id, slotDate, timeSlotId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Simulated exception", actual.Message);
            mockRepository.Verify(r => r.DeleteInterviewSlot(id, slotDate, timeSlotId), Times.Once);
        }

        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void GetTimeslots_ReturnsTimeslots_WhenTimeslotsExist()
        {
            // Arrange
            var Timeslot = new List<Timeslot>
            {
                new Timeslot { TimeslotId = 1, TimeslotName = "Timeslot1"},
                new Timeslot { TimeslotId = 2, TimeslotName = "Timeslot2"}
            };

            mockRepository.Setup(r => r.GetAllTimeslots()).Returns(Timeslot);

            var countryService = new InterviewService(mockRepository.Object);

            // Act
            var actual = countryService.GetAllTimeslots();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(Timeslot.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAllTimeslots(), Times.Once);
        }

        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void GetTimeslots_Returns_WhenNoTimeslotsExist()
        {
            // Arrange
            var Timeslot = new List<Timeslot>();


            mockRepository.Setup(r => r.GetAllTimeslots()).Returns(Timeslot);

            var countryService = new InterviewService(mockRepository.Object);

            // Act
            var actual = countryService.GetAllTimeslots();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetAllTimeslots(), Times.Once);
        }  

        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void GetTimeslots_ThrowsException()
        {
            // Arrange
            mockRepository.Setup(r => r.GetAllTimeslots()).Throws(new Exception("Simulated exception"));

            var countryService = new InterviewService(mockRepository.Object);

            // Act
            var actual = countryService.GetAllTimeslots();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Simulated exception", actual.Message);
            mockRepository.Verify(r => r.GetAllTimeslots(), Times.Once);
        }  
        
        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void GetInterviewslots_Returns_WhenInterviewslotsExist()
        {
            // Arrange
            var Timeslot = new List<InterviewSlots>
            {
                new InterviewSlots { TimeslotId = 1, SlotId = 1, EmployeeId=1},
                new InterviewSlots { TimeslotId = 2, SlotId = 2, EmployeeId=1}
            };

            mockRepository.Setup(r => r.GetAllInterviewslotsbyEmployeeId(1)).Returns(Timeslot);

            var countryService = new InterviewService(mockRepository.Object);

            // Act
            var actual = countryService.GetAllInterviewslotsbyEmployeeId(1);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(Timeslot.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAllInterviewslotsbyEmployeeId(1), Times.Once);
        }

        [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void GetInterviewslots_Returns_WhenNoSlotsExist()
        {
            // Arrange
            var Timeslot = new List<InterviewSlots>();

            mockRepository.Setup(r => r.GetAllInterviewslotsbyEmployeeId(1)).Returns(Timeslot);

            var countryService = new InterviewService(mockRepository.Object);

            // Act
            var actual = countryService.GetAllInterviewslotsbyEmployeeId(1);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetAllInterviewslotsbyEmployeeId(1), Times.Once);
        }
         [Fact]
        [Trait("Interview", "InterviewServiceTests")]
        public void GetInterviewslots_ThrowsError()
        {
            // Arrange
            mockRepository.Setup(r => r.GetAllInterviewslotsbyEmployeeId(1)).Throws(new Exception("Simulated exception"));

            var countryService = new InterviewService(mockRepository.Object);

            // Act
            var actual = countryService.GetAllInterviewslotsbyEmployeeId(1);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Simulated exception", actual.Message);
            mockRepository.Verify(r => r.GetAllInterviewslotsbyEmployeeId(1), Times.Once);
        }

        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

    }

}
