using InterviewPanelAvailabilitySystemAPI.Data;
using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Repositories
{
    public class RecruiterRepositoryTests : IDisposable
    {

        private readonly Mock<IAppDbContext> mockAppDbContext;

        public RecruiterRepositoryTests()
        {
            mockAppDbContext = new Mock<IAppDbContext>();
        }

        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetInterviewSlotsById_WhenInterviewersIsNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<InterviewSlots>().AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAppDbContext.SetupGet(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new RecruiterRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetInterviewSlotsById(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(5));
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.InterviewSlot, Times.Once);

        }
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetInterviewSlotsById_WhenInterviewersExists()
        {
            //Arrange
            var id = 1;
            var contacts = new List<InterviewSlots> {
     new InterviewSlots { SlotId = 1, EmployeeId = 1, IsBooked = false,SlotDate = DateTime.UtcNow,
            Timeslot=new Timeslot
             {
                 TimeslotId=1,
             },
                 Employee=new Employees{
                 JobRoleId=1,
                 JobRole =new JobRole{
                    JobRoleId =1,
                 },
                 InterviewRound=new InterviewRounds
                 {
                    InterviewRoundId=1,
                 }
             },
     }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAppDbContext.SetupGet(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new RecruiterRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetInterviewSlotsById(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(5));
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.InterviewSlot, Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetInterviewSlotsById_WhenExceptionOccured()
        {
            //Arrange
            var id = 1;
            var contacts = new List<InterviewSlots> {
     new InterviewSlots {
     }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAppDbContext.SetupGet(c => c.InterviewSlot).Throws(new Exception());
            var target = new RecruiterRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetInterviewSlotsById(id);
            //Assert
            Assert.Null(actual);
           mockAppDbContext.VerifyGet(c => c.InterviewSlot, Times.Once);
        }


        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void InterviewSlotExist_ReturnsTrue()
        {
            //Arrange
            var slotId = 1;
            bool isBooked = true;
            var contacts = new List<InterviewSlots>
            {
                 new InterviewSlots { SlotId = 1, EmployeeId = 1, IsBooked = true,SlotDate = DateTime.UtcNow,
            Timeslot=new Timeslot
             {
                 TimeslotId=1,
             },
                 Employee=new Employees{
                 JobRoleId=1,
                 JobRole =new JobRole{
                    JobRoleId =1,
                 },
                 InterviewRound=new InterviewRounds
                 {
                    InterviewRoundId=1,
                 }
             },
                 }

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new RecruiterRepository(mockAbContext.Object);

            //Act
            var actual = target.InterviewSlotExist(slotId, isBooked);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
        }


        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void InterviewSlotExists_ReturnsFalse()
        {
            //Arrange
            var slotId = 1;
            bool isBooked = true;
            var contacts = new List<InterviewSlots>().AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new RecruiterRepository(mockAppDbContext.Object);

            //Act
            var actual = target.InterviewSlotExist(slotId, isBooked);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.InterviewSlot, Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void InterviewSlotExists_ReturnsException()
        {
            //Arrange
            var slotId = 1;
            bool isBooked = true;
            var contacts = new List<InterviewSlots>().AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.InterviewSlot).Throws(new Exception());
            var target = new RecruiterRepository(mockAbContext.Object);

            //Act
            var actual = target.InterviewSlotExist(slotId, isBooked);
            //Assert
            Assert.False(actual);
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
        }

        //[Fact]
        //public void UpdateInterviewSlots_ReturnsTrue()
        //{
        //    // Arrange
        //    var mockDbSet = new Mock<DbSet<InterviewSlots>>();
        //    var mockAppDbContext = new Mock<IAppDbContext>();
        //    mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object); // Note: Use consistent property name
        //    mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);

        //    var target = new RecruiterRepository(mockAppDbContext.Object);

        //    var contact = new InterviewSlots
        //    {
        //        SlotId = 1,
        //        EmployeeId = 1,
        //        IsBooked = false,
        //        SlotDate = DateTime.UtcNow,
        //        Timeslot = new Timeslot
        //        {
        //            TimeslotId = 1,
        //        },
        //        Employee = new Employees
        //        {
        //            JobRoleId = 1,
        //            JobRole = new JobRole
        //            {
        //                JobRoleId = 1,
        //            },
        //            InterviewRound = new InterviewRounds
        //            {
        //                InterviewRoundId = 1,
        //            }
        //        },
        //    };

        //    // Act
        //    var actual = target.UpdateInterviewSlots(contact);

        //    // Assert
        //    Assert.True(actual);
        //    mockDbSet.Verify(c => c.Update(It.IsAny<InterviewSlots>()), Times.Once); // Verify the Update method call
        //}


        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void UpdateInterviewSlots_ReturnsFalse()
        {
            //Arrange
            InterviewSlots interviewSlot = null;
            var target = new RecruiterRepository(mockAppDbContext.Object);

            //Act
            var actual = target.UpdateInterviewSlots(interviewSlot);
            //Assert
            Assert.False(actual);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void UpdateInterviewSlots_ReturnsException()
        {
            //Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.InterviewSlot).Throws(new Exception());
            var target = new RecruiterRepository(mockAppDbContext.Object);

            //Act
            var actual = target.UpdateInterviewSlots(null);
            //Assert
            Assert.False(actual);
        }


        //GetPaginatedInterviwerByAll
        //1st case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetPaginatedInterviwerByAll_Returns_Correct_Pagination_And_Sorting()
        {
            // Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true } },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test 2", LastName = "last 2", JobRoleId = 2, InterviewRoundId = 1, IsActive = true } }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.GetPaginatedInterviwerByAll(page: 1, pageSize: 2, searchQuery: "Jane", sortOrder: "asc", jobRoleId: 2, roundId: 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
            mockAppDbContext.Verify(c => c.InterviewSlot, Times.Once);
        }


        //2nd case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetPaginatedInterviwerByAll_Returns_EmptyList_When_No_Records_Found()
        {
            // Arrange
            var mockDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>().AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.GetPaginatedInterviwerByAll(page: 1, pageSize: 10, searchQuery: "NonExistent", sortOrder: "asc", jobRoleId: null, roundId: null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            mockDbContext.Verify(c => c.InterviewSlot, Times.Once);
        }


        //3rd case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetPaginatedInterviwerByAll_Filters_ByRoundId_When_JobRoleId_Is_Null()
        {
            // Arrange
            var mockDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true } },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 2, InterviewRoundId = 2, IsActive = true } }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.GetPaginatedInterviwerByAll(page: 1, pageSize: 10, searchQuery: null, sortOrder: "asc", jobRoleId: null, roundId: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("test", result.First().Employee.FirstName);
            mockDbContext.Verify(c => c.InterviewSlot, Times.Once);
        }


        //4th casee
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetPaginatedInterviwerByAll_Filters_ByJobRoleId_When_RoundId_Is_Null()
        {
            // Arrange
            var mockDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true } },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 2, InterviewRoundId = 2, IsActive = true } }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.GetPaginatedInterviwerByAll(page: 1, pageSize: 10, searchQuery: null, sortOrder: "asc", jobRoleId: 2, roundId: null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("test", result.First().Employee.FirstName);
            mockDbContext.Verify(c => c.InterviewSlot, Times.Once);
        }


        //5th case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetPaginatedInterviwerByAll_When_InvalidSortingOrder()
        {
            // Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true } },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 2, InterviewRoundId = 2, IsActive = true } }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.GetPaginatedInterviwerByAll(page: 1, pageSize: 10, searchQuery: null, sortOrder: "aaasc", jobRoleId: 2, roundId: null);

            // Assert

            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Provider, Times.Exactly(5));
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.Expression, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.ElementType, Times.Never);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(m => m.GetEnumerator(), Times.Never);
        }


        //6th case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetPaginatedInterviwerByAll_SortsByFirstNameDescending()
        {
            // Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true } },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 2, InterviewRoundId = 2, IsActive = true } }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.GetPaginatedInterviwerByAll(page: 1, pageSize: 10, searchQuery: null, sortOrder: "desc", jobRoleId: null, roundId: null);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("test", result.First().Employee.FirstName);
            Assert.Equal("test", result.Last().Employee.FirstName);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void GetPaginatedInterviwerByAll_ReturnsERxception()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true } },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 2, InterviewRoundId = 2, IsActive = true } }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Throws(new Exception());

            // Act
            var result = mockRepository.GetPaginatedInterviwerByAll(page: 1, pageSize: 10, searchQuery: null, sortOrder: "desc", jobRoleId: null, roundId: null);

            // Assert
            Assert.Equal(0, result.Count());
            Assert.NotNull(result);
        }

        //TotalInterviewSlotsByAll
        //1st case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void TotalInterviewSlotsByAll_NoFilters()
        {
            // Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 2, InterviewRoundId = 2, IsActive = true }, IsBooked = false }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.TotalInterviewSlotsByAll(searchQuery: null, jobRoleId: 0, roundId: 0);

            // Assert
            Assert.Equal(0, result);
        }


        //2nd case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void TotalInterviewSlotsByAll_FiltersByJobRoleAndRound()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true }, IsBooked = false }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.TotalInterviewSlotsByAll(searchQuery: null, jobRoleId: 2, roundId: 1);

            // Assert
            Assert.Equal(0, result);
        }


        //3rd case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void TotalInterviewSlotsByAll_FiltersBySearchQuery()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test 1", LastName = "last 1", JobRoleId = 2, InterviewRoundId = 2, IsActive = true }, IsBooked = false }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.TotalInterviewSlotsByAll(searchQuery: "test", jobRoleId: 0, roundId: 0);

            // Assert
            Assert.Equal(0, result);
        }


        //4th case

        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void TotalInterviewSlotsByAll_FilterByRoundId()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test", LastName = "last 1", JobRoleId = 1, InterviewRoundId = 1, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test 2", LastName = "last 2", JobRoleId = 2, InterviewRoundId = 2, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 3, Employee = new Employees { FirstName = "test 3", LastName = "last 3", JobRoleId = 2, InterviewRoundId = 1, IsActive = true }, IsBooked = false }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.TotalInterviewSlotsByAll(searchQuery: null, jobRoleId: null, roundId: 1);

            // Assert
            Assert.Equal(2, result);
        }


        //5th case
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void TotalInterviewSlotsByAll_FilterByJobRoleId()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test ", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test 2", LastName = "last 2", JobRoleId = 2, InterviewRoundId = 2, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 3, Employee = new Employees { FirstName = "test 3", LastName = "last 3", JobRoleId = 2, InterviewRoundId = 1, IsActive = true }, IsBooked = false }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);

            // Act
            var result = mockRepository.TotalInterviewSlotsByAll(searchQuery: null, jobRoleId: 1, roundId: null);

            // Assert
            Assert.Equal(1, result);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterRepositoryTests")]
        public void TotalInterviewSlotsByAll_ReturnException()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockRepository = new RecruiterRepository(mockAppDbContext.Object);

            var interviewSlotsData = new List<InterviewSlots>
            {
                new InterviewSlots { SlotId = 1, Employee = new Employees { FirstName = "test ", LastName = "last", JobRoleId = 1, InterviewRoundId = 1, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 2, Employee = new Employees { FirstName = "test 2", LastName = "last 2", JobRoleId = 2, InterviewRoundId = 2, IsActive = true }, IsBooked = false },
                new InterviewSlots { SlotId = 3, Employee = new Employees { FirstName = "test 3", LastName = "last 3", JobRoleId = 2, InterviewRoundId = 1, IsActive = true }, IsBooked = false }
            }.AsQueryable();

            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Provider).Returns(interviewSlotsData.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.Expression).Returns(interviewSlotsData.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.ElementType).Returns(interviewSlotsData.ElementType);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(m => m.GetEnumerator()).Returns(interviewSlotsData.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewSlot).Throws(new Exception());

            // Act
            var result = mockRepository.TotalInterviewSlotsByAll(searchQuery: null, jobRoleId: 1, roundId: null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result);
        }


        public void Dispose()
        {
            mockAppDbContext.VerifyAll();
        }
    }
}
