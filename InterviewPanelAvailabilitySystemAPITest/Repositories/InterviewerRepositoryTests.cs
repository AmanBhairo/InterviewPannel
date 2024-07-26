using InterviewPanelAvailabilitySystemAPI.Controllers;
using InterviewPanelAvailabilitySystemAPI.Data;
using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace InterviewPanelAvailabilitySystemAPITest.Repositories
{
    public class InterviewerRepositoryTests : IDisposable
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;

        public InterviewerRepositoryTests()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

        }
        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]

        public void InsertInterviewSlot_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockAppDbContext.SetupGet(c => c.InterviewSlot).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new InterviewRepository(mockAppDbContext.Object);
            var slot = new InterviewSlots
            {
                SlotId = 1,
                 EmployeeId = 1
            };


            //Act
            var actual = target.AddInterviewSlot(slot);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(slot), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]

        public void InsertInterviewSlot_ReturnsFalse()
        {
            //Arrange
            InterviewSlots slot = null;
            var target = new InterviewRepository(mockAppDbContext.Object);

            //Act
            var actual = target.AddInterviewSlot(slot);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void InsertInterviewSlot_ThrowsException()
        {
            //Arrange
            mockAppDbContext.SetupGet(c => c.InterviewSlot).Throws(new Exception("Simulated exception"));
            var target = new InterviewRepository(mockAppDbContext.Object);
            var slot = new InterviewSlots
            {
                SlotId = 1,
                EmployeeId = 1
            };


            //Act
            var actual = target.AddInterviewSlot(slot);

            //Assert
            Assert.False(actual);
        }


        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]

        public void DeleteInterviewSlot_ReturnsTrue()
        {
            // Arrange
            var employeeId = 1;
            var slotDate = DateTime.Now.AddDays(2);
            var timeSlotId = 1;
            var slots = new List<InterviewSlots>
            {
               new InterviewSlots { EmployeeId = employeeId, SlotDate = slotDate, TimeslotId = timeSlotId }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<InterviewSlots>>();
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(slots.Provider);
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(slots.Expression);


            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1); // Assuming SaveChanges returns the number of affected rows

            var target = new InterviewRepository(mockAppDbContext.Object);

            // Act
            var result = target.DeleteInterviewSlot(employeeId, slotDate, timeSlotId);

            // Assert
            Assert.True(result);
            mockSet.Verify(m => m.Remove(It.IsAny<InterviewSlots>()), Times.Once());
            mockAppDbContext.Verify(m => m.SaveChanges(), Times.Once());
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Provider, Times.Once());
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Expression, Times.Once());
        }

        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]

        public void DeleteInterviewSlot_ReturnsFalse()
        {
            // Arrange
            var employeeId = 1;
            var slotDate = DateTime.Now.AddDays(2);
            var timeSlotId = 1;
            var slots = new List<InterviewSlots>
             {
                new InterviewSlots { EmployeeId = 2, SlotDate = slotDate, TimeslotId = timeSlotId }
             }.AsQueryable();

            var mockSet = new Mock<DbSet<InterviewSlots>>();
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(slots.Provider);
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(slots.Expression);


            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockSet.Object);
            var target = new InterviewRepository(mockAppDbContext.Object);

            // Act
            var actual = target.DeleteInterviewSlot(employeeId, slotDate, timeSlotId);

            // Assert
            Assert.False(actual);
            mockSet.Verify(m => m.Remove(It.IsAny<InterviewSlots>()), Times.Never());
            mockAppDbContext.Verify(m => m.SaveChanges(), Times.Never());
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Provider, Times.Once());
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Expression, Times.Once());
        }

         [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void DeleteInterviewSlot_ThrowsException()
        {
            // Arrange
            var employeeId = 1;
            var slotDate = DateTime.Now.AddDays(2);
            var timeSlotId = 1;
            mockAppDbContext.Setup(c => c.InterviewSlot).Throws(new Exception("Simulated exception"));
            var target = new InterviewRepository(mockAppDbContext.Object);

            // Act
            var actual = target.DeleteInterviewSlot(employeeId, slotDate, timeSlotId);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void InterviewSlotExist_ReturnsFalse_WhenSlotDoesNotExist()
        {
            // Arrange
            var employeeId = 1;
            var slotDate = DateTime.Now.AddDays(2);
            var timeSlotId = 1;
            var slots = new List<InterviewSlots>().AsQueryable(); // Empty list to simulate no matching slot

            var mockSet = new Mock<DbSet<InterviewSlots>>();
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(slots.Provider);
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(slots.Expression);


            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockSet.Object);
            var target = new InterviewRepository(mockAppDbContext.Object);

            // Act
            var actual = target.InterviewSlotExist(employeeId, slotDate, timeSlotId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual);
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Provider, Times.Once());
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Expression, Times.Once());
        } 
        
        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void InterviewSlotExist_ThrowsException()
        {
            // Arrange
            var employeeId = 1;
            var slotDate = DateTime.Now.AddDays(2);
            var timeSlotId = 1;
            mockAppDbContext.Setup(c => c.InterviewSlot).Throws(new Exception("Simulated exception"));
            var target = new InterviewRepository(mockAppDbContext.Object);

            // Act
            var actual = target.InterviewSlotExist(employeeId, slotDate, timeSlotId);

            // Assert
            Assert.False(actual);
          
        }
         
        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void InterviewSlotExist_ReturnsTrue_WhenSlotDoesNotExist()
        {
            // Arrange
            var employeeId = 1;
            var slotDate = DateTime.Now.AddDays(2);
            var timeSlotId = 1;
            var slots = new List<InterviewSlots>
    {
        new InterviewSlots { EmployeeId = 1, SlotDate = slotDate, TimeslotId = timeSlotId }
    }.AsQueryable();

            var mockSet = new Mock<DbSet<InterviewSlots>>();
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(slots.Provider);
            mockSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(slots.Expression);


            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockSet.Object);
            var target = new InterviewRepository(mockAppDbContext.Object);

            // Act
            var actual = target.InterviewSlotExist(employeeId, slotDate, timeSlotId);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual);
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Provider, Times.Once());
            mockSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Expression, Times.Once());
        }


        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void GetAll_ReturnsTimeslots_WhenTimeslotsExist()
        {
            var list = new List<Timeslot>
            {
              new Timeslot{ TimeslotId=1, TimeslotName="Timeslot 1"},
              new Timeslot{ TimeslotId=2, TimeslotName="Timeslot 2"},
             }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Timeslot>>();
            mockDbSet.As<IQueryable<Timeslot>>().Setup(c => c.GetEnumerator()).Returns(list.GetEnumerator());

            mockAppDbContext.Setup(c => c.Timeslot).Returns(mockDbSet.Object);
            var target = new InterviewRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllTimeslots();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(list.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.Timeslot, Times.Once);
            mockDbSet.As<IQueryable<Timeslot>>().Verify(c => c.GetEnumerator(), Times.Once);

        }

        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void GetAll_ReturnsEmpty_WhenNoTimeslotsExist()
        {
            var list = new List<Timeslot>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Timeslot>>();
            mockDbSet.As<IQueryable<Timeslot>>().Setup(c => c.GetEnumerator()).Returns(list.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Timeslot).Returns(mockDbSet.Object);
            var target = new InterviewRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllTimeslots();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(list.Count(), actual.Count());
            mockAbContext.Verify(c => c.Timeslot, Times.Once);
            mockDbSet.As<IQueryable<Timeslot>>().Verify(c => c.GetEnumerator(), Times.Once);

        }   
        
        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void GetAll_ThrowsException()
        {
            var list = new List<Timeslot>().AsQueryable();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Timeslot).Throws(new Exception());
            var target = new InterviewRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllTimeslots();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(list.Count(), actual.Count());

        }

        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void GetAllInterviewslotsbyEmployeeId_ReturnsSlots_WhenExist()
        {
            var list = new List<InterviewSlots>
            {
              new InterviewSlots{ TimeslotId=1, EmployeeId=2},
              new InterviewSlots{ TimeslotId=2, EmployeeId=2},
             }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(list.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(list.Expression);
            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new InterviewRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllInterviewslotsbyEmployeeId(2);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(list.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Provider, Times.Once());
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Expression, Times.Once());

        }  
        
        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void GetAllInterviewslotsbyEmployeeId_ReturnsEmptySlots_WhenNotExist()
        {
            var list = new List<InterviewSlots>
            ().AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(list.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(list.Expression);
            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new InterviewRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllInterviewslotsbyEmployeeId(2);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(list.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Provider, Times.Once());
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Expression, Times.Once());

        }
          
        [Fact]
        [Trait("Interviewer", "InterviewerRepositoryTests")]
        public void GetAllInterviewslotsbyEmployeeId_ThrowsException()
        {
            var list = new List<InterviewSlots>
            ().AsQueryable();
            mockAppDbContext.Setup(c => c.InterviewSlot).Throws(new Exception());
            var target = new InterviewRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetAllInterviewslotsbyEmployeeId(2);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(list.Count(), actual.Count());         

        }


        public void Dispose()
        {
            mockAppDbContext.VerifyAll();
        }

    }
}
