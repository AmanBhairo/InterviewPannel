using InterviewPanelAvailabilitySystemAPI.Data;
using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Implementation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Repositories
{
    public class AdminRepositoryTests : IDisposable
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;

        public AdminRepositoryTests()
        {
            mockAppDbContext = new Mock<IAppDbContext>();
        }


        //UpdateEmployee
        //1st case
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void UpdateEmployee_ReturnsTrue_WhenEmployeeUpdatedSuccessfully()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockAppDbContext.SetupGet(c => c.Employee).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new AdminRepository(mockAppDbContext.Object);
            var employees = new Employees
            {
                EmployeeId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.UpdateEmployee(employees);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(employees), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void UpdateEmployee_ReturnsFalse_WhenEmployeeUpdationFails()
        {
            //Arrange
            Employees employee = null;
            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.UpdateEmployee(employee);
            //Assert
            Assert.False(actual);
        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void UpdateEmployee_ExceptionThrown_ReturnsFalse()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Employees>>();
            mockAppDbContext.Setup(c => c.Employee).Returns(mockSet.Object);

            var repository = new AdminRepository(mockAppDbContext.Object);

            var employee = new Employees
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            mockAppDbContext.Setup(c => c.SaveChanges()).Throws(new DbUpdateException("Simulated database exception"));

            // Act
            var result = repository.UpdateEmployee(employee);

            // Assert
            Assert.False(result); 
        }




        //GetEmployeeByEmployeeIdAndEmail
        //1st case
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetEmployeeByEmployeeIdAndEmail_ReturnsEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employees = new List<Employees>()
            {
                new Employees() { EmployeeId = 1, Email = "Test@gmail.com" },
                new Employees() { EmployeeId = 2, Email = "Test@gmsil.com" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Employees>>();
            // This line sets up the Provider property of the mocked DbSet<Category> to return the Provider property of the categories IQueryable collection when accessed.
            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(employees.Provider);

            // This line sets up the Expression property of the mocked DbSet<Category> to return the Expression property of the categories IQueryable collection when accessed.
            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(employees.Expression);

            mockAppDbContext.SetupGet(c => c.Employee).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);
            var employeeId = 1;
            var email = "test@gmail.com";

            // Act 
            var actual = target.GetEmployeeByEmployeeIdAndEmail(employeeId, email);

            // Assert
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Employee, Times.Once);

        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetEmployeeByEmployeeIdAndEmail_ExceptionOccurs_ReturnsDefaultEmployee()
        {
            // Arrange
            var mockDbContext = new Mock<IAppDbContext>(); 
            var employeeService = new AdminRepository(mockDbContext.Object);
            mockDbContext.Setup(x => x.Employee).Throws(new Exception("Simulated exception"));

            // Act
            var result = employeeService.GetEmployeeByEmployeeIdAndEmail(1, "test@example.com");

            // Assert
            Assert.NotNull(result);
        }


        //GetEmployeeById
        //1st case
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetEmployeeById_ReturnsEmployee_WhenEmployeeExists()
        {
            //Arrange
            var employee = new Employees { EmployeeId = 1, FirstName = "Test 1" };
            var categories = new List<Employees> {
                employee,
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employees>>();

            // This line sets up the Provider property of the mocked DbSet<Category> to return the Provider property of the categories IQueryable collection when accessed.
            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(categories.Provider);

            // This line sets up the Expression property of the mocked DbSet<Category> to return the Expression property of the categories IQueryable collection when accessed.
            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(categories.Expression);
            var id = 1;
            mockAppDbContext.SetupGet(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetEmployeeById(id);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(employee, actual);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Employee, Times.Once);
        }

        //2nd case
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetEmployeeById_ReturnsNull_WhenEmployeeNotExists()
        {
            //Arrange
            var employee = new Employees { EmployeeId = 1, FirstName = "Test 1" };
            var categories = new List<Employees> {
                employee,
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employees>>();

            // This line sets up the Provider property of the mocked DbSet<Category> to return the Provider property of the categories IQueryable collection when accessed.
            // The Provider property is used by LINQ query execution.
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(categories.Provider);

            // This line sets up the Expression property of the mocked DbSet<Category> to return the Expression property of the categories IQueryable collection when accessed.
            // The Expression property represents the LINQ expression tree associated with the IQueryable collection.
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(categories.Expression);
            var id = 3;
            mockAppDbContext.SetupGet(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetEmployeeById(id);

            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.VerifyGet(c => c.Employee, Times.Once);
        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetEmployeeById_Exception_ReturnsNull()
        {
            // Arrange
            var employeeId = 1;
            mockAppDbContext.Setup(c => c.Employee).Throws(new Exception("Simulated exception"));
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var actual = target.GetEmployeeById(employeeId);

            // Assert
            Assert.Null(actual);

        }


        // GetAllEmployees
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllEmployees_ReturnsCorrectEmployees_WhenEmployeeExists_SearchIsNull()
        {
            string sortOrder = "asc";
            var employees = new List<Employees>
            {
                new Employees{ EmployeeId=1, FirstName="Employee 1", IsActive = true },
                new Employees{ EmployeeId=2, FirstName="Employee 2", IsActive = true },
                new Employees{ EmployeeId=3, FirstName="Employee 3", IsActive = true },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(employees.Expression);
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(employees.Provider);
            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllEmployees(1, 2, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Exactly(3));
        }
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllEmployees_ReturnsCorrectEmployees_WhenEmployeesExists_SearchIsNotNull()
        {
            string sortOrder = "desc";
            var employees = new List<Employees>
            {
                new Employees{ EmployeeId=1, FirstName="Employee 1", IsActive = true },
                new Employees{ EmployeeId=2, FirstName="Employee 2", IsActive = true },
                new Employees{ EmployeeId=3, FirstName="Employee 3", IsActive = true },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(employees.Expression);
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(employees.Provider);

            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllEmployees(1, 2, "Employee", sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockAppDbContext.Verify(c => c.Employee, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Exactly(3));
        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllEmployees_ExceptionThrown_ReturnsEmptyEnumerable()
        {
            // Arrange
            string sortOrder = "desc";
            var employeesData = new List<Employees>
            {
                new Employees { EmployeeId = 1, FirstName = "test", LastName = "test last", IsAdmin = false, IsActive = true },
                new Employees { EmployeeId = 2, FirstName = "test", LastName = "test last", IsAdmin = false, IsActive = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Employees>>();
            mockSet.As<IQueryable<Employees>>().Setup(m => m.Provider).Throws(new Exception("Simulated database exception"));

          

            var target = new AdminRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllEmployees(1, 2, "Employee", sortOrder);

            // Assert
            Assert.NotNull(actual);
        }
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllEmployees_ReturnsCorrectEmployees_WhenEmployeesExists_SearchIsNotNull_defaultOrder()
        {
            string sortOrder = "drfgh";
            var employees = new List<Employees>
            {
                new Employees{ EmployeeId=1, FirstName="Employee 1", IsActive = true },
                new Employees{ EmployeeId=2, FirstName="Employee 2", IsActive = true },
                new Employees{ EmployeeId=3, FirstName="Employee 3", IsActive = true },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(employees.Expression);
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(employees.Provider);

            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetAllEmployees(1, 2, "Employee", sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Exactly(3));
        }




        // TotalEmployeeCount
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void TotalEmployeeCount_ReturnsCount_WhenEmployeesExistWhenSearchIsNotNull()
        {
            string search = "Employee";
            var employees = new List<Employees> {
                new Employees{ EmployeeId=1, FirstName="Employee 1", IsActive = true },
                new Employees{ EmployeeId=2, FirstName="Employee 2", IsActive = true },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Provider).Returns(employees.Provider);
            mockDbSet.As<IQueryable<Employees>>().Setup(c => c.Expression).Returns(employees.Expression);
            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalEmployeeCount(search);

            //Assert
            Assert.Equal(employees.Count(), actual);
            mockAppDbContext.Verify(c => c.Employee, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Employees>>().Verify(c => c.Expression, Times.Once);

        }


        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]

        public void TotalEmployeeCount_Exception_ReturnsZero()
        {
            // Arrange
            string search = "test";
            var mockDbSet = new Mock<DbSet<Employees>>();
            mockAppDbContext.Setup(c => c.Employee).Throws(new Exception("Simulated database exception"));
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.TotalEmployeeCount(search);

            // Assert
            Assert.Equal(0, result); 
        }




        // EmployeeExists
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void EmployeeExists_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            var email = "existingEmail@test.com";
            var isActive = true;
            var usersData = new List<Employees> { new Employees { Email = email, IsActive = isActive },}.AsQueryable();

            var mockDbSet = new Mock<DbSet<Employees>>();
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.Provider).Returns(usersData.Provider);
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.Expression).Returns(usersData.Expression);

            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.EmployeeExists(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void EmployeeExists_Exception_ReturnsFalse()
        {
            // Arrange
            string email = "test@example.com";
            mockAppDbContext.Setup(c => c.Employee)
                            .Throws(new Exception("Simulated database exception"));
            var employeeService = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = employeeService.EmployeeExists(email);

            // Assert
            Assert.False(result); 
        }


        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void UserExists_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "existingEmail@test.com";
            var usersData = new List<Employees>{ }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Employees>>();
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.Provider).Returns(usersData.Provider);
            mockDbSet.As<IQueryable<Employees>>().Setup(m => m.Expression).Returns(usersData.Expression);

            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.EmployeeExists(email);

            // Assert
            Assert.False(result);
        }




        // AddEmployee
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void AddEmployee_ReturnsTrue_WhenUserIsNotNull()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Employees>>();
            var employee = new Employees
            {
                FirstName = "First name",
                LastName = "Last name",
                Email = "xyz@gmail.com",
                IsActive = true
            };
            mockAppDbContext.Setup(c => c.Employee).Returns(mockDbSet.Object);

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.AddEmployee(employee);

            // Assert
            Assert.True(result); 
            mockDbSet.Verify(m => m.Add(employee), Times.Once);
            mockAppDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void AddEmployee_ReturnsFalse_WhenUserIsNull()
        {
            // Arrange
            Employees employee = null;
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.AddEmployee(employee);

            // Assert
            Assert.False(result);
            mockAppDbContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void AddEmployee_ExceptionThrown_ReturnsFalse()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Employees>>();
            mockAppDbContext.Setup(c => c.Employee).Returns(mockSet.Object);

            var repository = new AdminRepository(mockAppDbContext.Object);

            var employee = new Employees
            {
                EmployeeId = 1,
                FirstName = "test",
                LastName = "test last",
                Email = "test@example.com"
            };

            mockAppDbContext.Setup(c => c.Employee)
                            .Throws(new Exception("Simulated database exception"));

            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.AddEmployee(employee);

            // Assert
            Assert.False(result);
        }



        // GetAllJobRoles
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllJobRoles_ReturnsJobRoles_WhenJobRolesExists()
        {
            // Arrange
            var jobRoles = new List<JobRole>()
            {
                new JobRole{ JobRoleId= 1, JobRoleName = "Jobrole1" },
                new JobRole{ JobRoleId= 2, JobRoleName = "Jobrole2" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<JobRole>>();
            mockDbSet.As<IQueryable<JobRole>>().Setup(c => c.GetEnumerator()).Returns(jobRoles.GetEnumerator());

            mockAppDbContext.Setup(c => c.JobRole).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act 
            var actual = target.GetAllJobRoles();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(jobRoles.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.JobRole, Times.Once);
            mockDbSet.As<IQueryable<JobRole>>().Verify(c => c.GetEnumerator(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllJobRoles_ReturnsEmptyList_WhenJobRolesNotExists()
        {
            // Arrange
            var jobRoles = new List<JobRole>().AsQueryable();
            var mockDbSet = new Mock<DbSet<JobRole>>();
            mockDbSet.As<IQueryable<JobRole>>().Setup(c => c.GetEnumerator()).Returns(jobRoles.GetEnumerator());

            mockAppDbContext.Setup(c => c.JobRole).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act 
            var actual = target.GetAllJobRoles();

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(jobRoles.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.JobRole, Times.Once);
            mockDbSet.As<IQueryable<JobRole>>().Verify(c => c.GetEnumerator(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]

        public void GetAllJobRoles_Exception_ReturnsEmptyEnumerable()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<JobRole>>();
            mockAppDbContext.Setup(c => c.JobRole).Throws(new Exception("Simulated database exception"));
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.GetAllJobRoles();

            // Assert
            Assert.Empty(result); 
        }


        // GetAllInterviewRounds
        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllInterviewRounds_ReturnsInterviewRounds_WhenInterviewRoundsExists()
        {
            // Arrange
            var interviewRounds = new List<InterviewRounds>()
            {
                new InterviewRounds{ InterviewRoundId= 1, InterviewRoundName = "Interviewround1" },
                new InterviewRounds{ InterviewRoundId= 2, InterviewRoundName = "Interviewround2" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<InterviewRounds>>();
            mockDbSet.As<IQueryable<InterviewRounds>>().Setup(c => c.GetEnumerator()).Returns(interviewRounds.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewRound).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act 
            var actual = target.GetAllInterviewRounds();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(interviewRounds.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.InterviewRound, Times.Once);
            mockDbSet.As<IQueryable<InterviewRounds>>().Verify(c => c.GetEnumerator(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllInterviewRounds_ReturnsEmptyList_WhenInterviewRoundsNotExists()
        {
            // Arrange
            var interviewRounds = new List<InterviewRounds>().AsQueryable();
            var mockDbSet = new Mock<DbSet<InterviewRounds>>();
            mockDbSet.As<IQueryable<InterviewRounds>>().Setup(c => c.GetEnumerator()).Returns(interviewRounds.GetEnumerator());

            mockAppDbContext.Setup(c => c.InterviewRound).Returns(mockDbSet.Object);
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act 
            var actual = target.GetAllInterviewRounds();

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(interviewRounds.Count(), actual.Count());
            mockAppDbContext.Verify(c => c.InterviewRound, Times.Once);
            mockDbSet.As<IQueryable<InterviewRounds>>().Verify(c => c.GetEnumerator(), Times.Once);
        }


        [Fact]
        [Trait("Admin", "AdminRepositoryTests")]
        public void GetAllInterviewRounds_Exception_ReturnsEmptyEnumerable()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<InterviewRounds>>();
            mockAppDbContext.Setup(c => c.InterviewRound).Throws(new Exception("Simulated database exception"));
            var target = new AdminRepository(mockAppDbContext.Object);

            // Act
            var result = target.GetAllInterviewRounds();

            // Assert
            Assert.Empty(result);
        }



        public void Dispose()
        {
            mockAppDbContext.VerifyAll();
        }
    }
}
