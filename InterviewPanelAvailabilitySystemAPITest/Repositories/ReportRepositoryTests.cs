using InterviewPanelAvailabilitySystemAPI.Data;
using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Repositories
{
    public class ReportRepositoryTests : IDisposable
    {
        private readonly Mock<IAppDbContext> mockAbContext;

        public ReportRepositoryTests()
        {
            mockAbContext = new Mock<IAppDbContext>();
        }

        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void GetSlotsCountReport_WhenJobRoleIsAvailable()
        {
            //Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            var contacts = new List<InterviewSlots>()
            {
             new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    SlotDate = DateTime.Now,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    SlotDate=DateTime.Parse("2024-07-17"),
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        InterviewRoundId=2
                    }
                }
            }.AsQueryable();

            var employee = new List<Employees>
            {
                new Employees{EmployeeId =1 ,JobRoleId=1,InterviewRoundId =1},
                new Employees{EmployeeId =2 ,JobRoleId=2,InterviewRoundId =2}

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);
            //Act
            var actual = target.SlotsCountReport(jobRoleId, interViewRoundId, startDate, endDate);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual.AvailableSlot);
            Assert.Equal(0, actual.BookedSlot);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(4));
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Exactly(2));
            mockAbContext.VerifyGet(c => c.InterviewSlot, Times.Exactly(2));

        }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void GetSlotsCountReport_WhenInterviewRoundIsAvailable()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = 1;
            DateTime? startDate = null;
            DateTime? endDate = null;
            var contacts = new List<InterviewSlots>()
            {
             new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    SlotDate = DateTime.Now,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    SlotDate=DateTime.Parse("2024-07-17"),
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        InterviewRoundId=2
                    }
                }
            }.AsQueryable();

            var employee = new List<Employees>
            {
                new Employees{EmployeeId =1 ,JobRoleId=1,InterviewRoundId =1},
                new Employees{EmployeeId =2 ,JobRoleId=2,InterviewRoundId =2}

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);
            //Act
            var actual = target.SlotsCountReport(jobRoleId, interViewRoundId, startDate, endDate);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual.AvailableSlot);
            Assert.Equal(0, actual.BookedSlot);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(4));
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Exactly(2));
            mockAbContext.VerifyGet(c => c.InterviewSlot, Times.Exactly(2));

        }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void GetSlotsCountReport_WhenDateRangeIsAvailable()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = null;
            DateTime? startDate = DateTime.Parse("2024-07-16");
            DateTime? endDate = DateTime.Parse("2024-07-18");
            var contacts = new List<InterviewSlots>()
            {
             new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    SlotDate = DateTime.Now,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    SlotDate=DateTime.Parse("2024-07-17"),
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        InterviewRoundId=2,
                        IsActive=true
                    }
                }
            }.AsQueryable();

            var employee = new List<Employees>
            {
                new Employees{EmployeeId =1 ,JobRoleId=1,InterviewRoundId =1},
                new Employees{EmployeeId =2 ,JobRoleId=2,InterviewRoundId =2}

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);
            //Act
            var actual = target.SlotsCountReport(jobRoleId, interViewRoundId, startDate, endDate);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.AvailableSlot);
            Assert.Equal(1, actual.BookedSlot);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(4));
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Exactly(2));
            mockAbContext.VerifyGet(c => c.InterviewSlot, Times.Exactly(2));

        }
        [Fact]
        public void GetSlotsCountReport_ReturnsException()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = null;
            DateTime? startDate = DateTime.Parse("2024-07-16");
            DateTime? endDate = DateTime.Parse("2024-07-18");
            var contacts = new List<InterviewSlots>()
            {
             new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    SlotDate = DateTime.Now,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    SlotDate=DateTime.Parse("2024-07-17"),
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        InterviewRoundId=2,
                        IsActive=true
                    }
                }
            }.AsQueryable();

            var employee = new List<Employees>
            {
                new Employees{EmployeeId =1 ,JobRoleId=1,InterviewRoundId =1},
                new Employees{EmployeeId =2 ,JobRoleId=2,InterviewRoundId =2}

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.InterviewSlot).Throws(new Exception());
            var target = new ReportRepository(mockAbContext.Object);
            //Act
            var actual = target.SlotsCountReport(jobRoleId, interViewRoundId, startDate, endDate);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.AvailableSlot);
           
        }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void ReportDetai_ReturnsReport_WhenJobRoleIsAvailable()
        {
            //Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page =1;
            int pageSize = 6;
            var contacts = new List<InterviewSlots>
            {
                new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = false,
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =1,
                        IsActive=true
                    }
                }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
    
            mockAbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked,page,pageSize);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual.Count());
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(3));

        }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void ReportDetai_ReturnsReport_WhenInterviewRoundIsAvailable()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = 1;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 6;
            var contacts = new List<InterviewSlots>
            {
                new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = false,
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
     
            mockAbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual.Count());
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(3));

        }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void ReportDetai_ReturnsReport_WhenDateRangeIsAvailable()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = null;
            DateTime? startDate = DateTime.Parse("2024-07-16");
            DateTime? endDate = DateTime.Parse("2024-07-18");
            bool booked = true;
            int page = 1;
            int pageSize = 6;
            var contacts = new List<InterviewSlots>
            {
                new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    SlotDate=DateTime.Parse("2024-07-17"),
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        InterviewRoundId=2,
                        IsActive=true
                    }
                }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockAbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual.Count());
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(3));

        }
        [Fact]
        public void ReportDetai_ReturnsException()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = null;
            DateTime? startDate = DateTime.Parse("2024-07-16");
            DateTime? endDate = DateTime.Parse("2024-07-18");
            bool booked = true;
            int page = 1;
            int pageSize = 6;
            var contacts = new List<InterviewSlots>
            {
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
           var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.InterviewSlot).Throws(new Exception());
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.ReportDetai(jobRoleId, interViewRoundId, startDate, endDate, booked, page, pageSize);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void totalReportDetailCount_ReturnsCount_WhenJobRoleIsAvailable()
        {
            //Arrange
            int? jobRoleId = 1;
            int? interViewRoundId = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            var contacts = new List<InterviewSlots>
            {
                new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        IsActive=true
                    }
                }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockAbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual);
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(2));

        }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void totalReportDetailCount_ReturnsCount_WhenInterviewRoundIsAvailable()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = 1;
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool booked = false;
            var contacts = new List<InterviewSlots>
            {
                new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                        IsActive=true
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        InterviewRoundId=2,
                        IsActive=true
                    }
                }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
     
            mockAbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual);
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(2));
        }
        [Fact]
        [Trait("Report", "ReportRepositoryTests")]
        public void totalReportDetailCount_ReturnsCount_WhenDateRangeIsAvailable()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = null;
            DateTime? startDate = DateTime.Now;
            DateTime? endDate = DateTime.Now;
            bool booked = false;
            var contacts = new List<InterviewSlots>
            {
                new InterviewSlots
                {
                    SlotId = 1,
                    TimeslotId = 1,
                    IsBooked = false,
                    SlotDate = DateTime.Now,
                    Employee= new Employees
                    {
                        EmployeeId=1,
                        JobRoleId =1,
                        InterviewRoundId=1,
                    }
                },
                new InterviewSlots
                {
                    SlotId = 2,
                    TimeslotId =2,
                    IsBooked = true,
                    Employee= new Employees
                    {
                        EmployeeId=2,
                        JobRoleId =2,
                        InterviewRoundId=2
                    }
                }
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<InterviewSlots>>().Setup(c => c.Provider).Returns(contacts.Provider);
      
            mockAbContext.Setup(c => c.InterviewSlot).Returns(mockDbSet.Object);
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual);
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<InterviewSlots>>().Verify(c => c.Provider, Times.Exactly(2));
        }
        [Fact]
        public void totalReportDetailCount_ReturnsException()
        {
            //Arrange
            int? jobRoleId = null;
            int? interViewRoundId = null;
            DateTime? startDate = DateTime.Now;
            DateTime? endDate = DateTime.Now;
            bool booked = false;
            var contacts = new List<InterviewSlots>
            {
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewSlots>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.InterviewSlot).Throws(new Exception());
            var target = new ReportRepository(mockAbContext.Object);

            //Act
            var actual = target.totalReportDetailCount(jobRoleId, interViewRoundId, startDate, endDate, booked);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual);
            mockAbContext.Verify(c => c.InterviewSlot, Times.Once);
            }

        public void Dispose()
        {
            mockAbContext.VerifyAll();
        }
    }
}
