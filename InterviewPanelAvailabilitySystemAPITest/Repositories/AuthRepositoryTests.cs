using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPITest.Repositories
{
    public class AuthRepositoryTests : IDisposable
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;

        public AuthRepositoryTests()
        {
            mockAppDbContext = new Mock<IAppDbContext>();
        }
        // UpdateUser
        [Fact]
        [Trait("Auth", "AuthRepositoryTests")]
        public void UpdateUser_ReturnsTrue()
        {
            // Arrange
            var employee = new Employees
            {
                EmployeeId = 2,
                FirstName = "TestName",
                Email = "Test@test.com",
            };
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUser(employee);

            // Assert
            Assert.True(actual);
            mockDbSet.Verify(p => p.Update(employee), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthRepositoryTests")]
        public void UpdateUser_ReturnsFalse()
        {
            // Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            var target = new AuthRepository(mockAppDbContext.Object);
            Employees employee = null;

            // Act
            var actual = target.UpdateUser(employee);

            // Assert
            Assert.False(actual);
        }
        [Fact]
        [Trait("Auth", "AuthRepositoryTests")]
        public void UpdateUser_ReturnsException()
        {

            // Arrange
            var employee = new Employees
            {
                EmployeeId = 2,
                FirstName = "TestName",
                Email = "Test@test.com",
            };
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockAppDbContext.Setup(c => c.Employee).Throws(new Exception());
            var target = new AuthRepository(mockAppDbContext.Object);

            // Act
            var actual = target.UpdateUser(employee);

            // Assert
            Assert.NotNull(actual);
        }
        [Fact]
        [Trait("Auth", "AuthRepositoryTests")]
        public void ValidateUser_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "notExistingEmail@123";
            var userData = new List<Employees>
            { }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Employees>>();
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            var mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(db => db.Employee).Returns(mockDbSet.Object);

            var target = new AuthRepository(mockDbContext.Object);

            // Act
            var result = target.ValidateUser(email);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        [Trait("Auth", "AuthRepositoryTests")]
        public void ValidateUser_ReturnsUser_WhenUserExists()
        {
            //Arrange
            var users = new List<Employees>
            {
                new Employees
            {
                EmployeeId = 2,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                },
                new Employees
            {
                EmployeeId = 3,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email1@example.com",
                },
            }.AsQueryable();
            var email = "email@example.com";
            var mockDbSet = new Mock<DbSet<Employees>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.ValidateUser(email);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Employee, Times.Once);
        }
        [Fact]
        [Trait("Auth", "AuthRepositoryTests")]
        public void ValidateUser_ReturnsException()
        {
            //Arrange
            var users = new List<Employees>
            {
            }.AsQueryable();
            var email = "email@example.com";
            var mockDbSet = new Mock<DbSet<Employees>>();
            var mockAbContext = new Mock<IAppDbContext>();
           mockAbContext.SetupGet(c => c.Employee).Throws(new Exception());
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.ValidateUser(email);
            //Assert
            Assert.NotNull(actual);
           mockAbContext.VerifyGet(c => c.Employee, Times.Once);
        }
        public void Dispose()
        {
            mockAppDbContext.VerifyAll();
        }
    }
}
