using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using InterviewPanelAvailabilitySystemAPI.Services.Implementation;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Services
{
    public class AdminServiceTests : IDisposable
    {
        private readonly Mock<IAdminRepository> mockAdminRepository;
        private readonly Mock<IPasswordService> mockPasswordService;

        public AdminServiceTests()
        {
            mockAdminRepository = new Mock<IAdminRepository>();
            mockPasswordService = new Mock<IPasswordService>();
        }


        //UpdateEmployee
        //1st case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UpdateEmployee_AlreadyExists_ReturnsFailure()
        {
            // Arrange
            var employees = new UpdateEmployeeDtos
            {
                EmployeeId = 1,
                Email = "existing.email@example.com"
            };

            mockAdminRepository.Setup(repo => repo.GetEmployeeById(employees.EmployeeId))
                                .Returns(new Employees { EmployeeId = employees.EmployeeId });
            var adminService = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = adminService.UpdateEmployee(employees);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", result.Message);
        }
       


        //2nd case  -- if (employees == null)
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UpdateEmployee_NullEmployees_ReturnsFailure()
        {
            // Arrange
            UpdateEmployeeDtos employees = null;
            var mockAdminService = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = mockAdminService.UpdateEmployee(employees);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", result.Message);
        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UpdateEmployee_WhenEmployeeNotFound_ShouldReturnFailure()
        {
            // Arrange
            var mockRepository = new Mock<IAdminRepository>();
            var service = new AdminService(mockRepository.Object, mockPasswordService.Object);

            var updateDto = new UpdateEmployeeDtos
            {
                EmployeeId = 1,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "new.email@example.com",
                JobRoleId = 2,
                InterviewRoundId = 3
            };

            mockRepository.Setup(r => r.GetEmployeeById(updateDto.EmployeeId))
                          .Returns((Employees)null); // Simulate employee not found

            // Act
            var result = service.UpdateEmployee(updateDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", result.Message);
            mockRepository.Verify(r => r.GetEmployeeById(updateDto.EmployeeId), Times.Once);
        }


        //4th case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UpdateEmployee_UpdateFailure_ReturnsFailure()
        {
            // Arrange
            var employees = new UpdateEmployeeDtos
            {
                EmployeeId = 1,
                FirstName = "Test",
                LastName = "test lastname",
                Email = "test@gmail.com",
                JobRoleId = 2,
                InterviewRoundId = 3
            };

            mockAdminRepository.Setup(repo => repo.GetEmployeeById(employees.EmployeeId))
                                .Returns(new Employees { EmployeeId = employees.EmployeeId });

            mockAdminRepository.Setup(repo => repo.UpdateEmployee(It.IsAny<Employees>()))
                                .Returns(false);
            var mockAdminService = new Mock<IAdminService>();
            // Act
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var actual = target.UpdateEmployee(employees);
            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", actual.Message);
        }


        //5th case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UpdateEmployee_Exception_ReturnsErrorResponse()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDtos
            {
                EmployeeId = 1,
                FirstName = "test",
                LastName = "last",
                Email = "test@example.com",
                JobRoleId = 1,
                InterviewRoundId = 1
            };
            mockAdminRepository.Setup(repo => repo.GetEmployeeById(updateEmployeeDto.EmployeeId))
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Simulated repository exception", result.Message); 
        }


        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void UpdateEmployee_AdminIsNotUpdated_ReturnsErrorMessage()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDtos
            {
                EmployeeId = 123,
                FirstName = "Test",
                LastName = "Last test",
                Email = "test@example.com",
                JobRoleId = 2,
                InterviewRoundId = 1
            };
            var mockRepository = new Mock<IAdminRepository>();
            var adminEmployee = new Employees
            {
                EmployeeId = 123,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@example.com",
                JobRoleId = 1,  
                InterviewRoundId = 1,
                IsAdmin = true
            };
            mockRepository.Setup(r => r.GetEmployeeById(123)).Returns(adminEmployee);
            var service = new AdminService(mockRepository.Object, mockPasswordService.Object);

            // Act
            var response = service.UpdateEmployee(updateEmployeeDto);

            // Assert
            Assert.False(response.Success);  
            Assert.Equal("Admin can't updated.", response.Message);  
        }




        //DeleteEmployee
        //1st case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DeleteEmployee_ValidDeletion_ReturnsSuccess()
        {
            // Arrange
            var EmployeeId = 1;

            mockAdminRepository.Setup(repo => repo.GetEmployeeById(EmployeeId))
                                .Returns(new Employees { EmployeeId = EmployeeId });

            mockAdminRepository.Setup(repo => repo.UpdateEmployee(It.IsAny<Employees>()))
                                .Returns(true);
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var actual = target.DeleteEmployee(EmployeeId);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal("Employee deleted successfully.", actual.Message);
        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DeleteEmployee_EmployeeNotFound_ReturnsFailure()
        {
            // Arrange
            var EmployeeId = 999;


            mockAdminRepository.Setup(repo => repo.GetEmployeeById(EmployeeId))
                                .Returns((Employees)null);
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var actual = target.DeleteEmployee(EmployeeId);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Employee not found.", actual.Message);
        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DeleteEmployee_UpdateFailure_ReturnsFailure()
        {
            // Arrange
            var EmployeeId = 1;


            mockAdminRepository.Setup(repo => repo.GetEmployeeById(EmployeeId))
                                .Returns(new Employees { EmployeeId = EmployeeId });

            mockAdminRepository.Setup(repo => repo.UpdateEmployee(It.IsAny<Employees>()))
                                .Returns(false);
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var actual = target.DeleteEmployee(EmployeeId);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong. Please try after sometime.", actual.Message);
        }



        //4th case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void DeleteEmployee_Exception_ReturnsErrorResponse()
        {
            // Arrange
            int id = 1;
            mockAdminRepository.Setup(repo => repo.GetEmployeeById(id))
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.DeleteEmployee(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Simulated repository exception", result.Message); 
        }





        //GetEmployeeById
        //1st case
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetEmployeeById_ValidId_ReturnsEmployeeDto()
        {
            // Arrange
            int employeeId = 1;
            var mockEmployee = new Employees
            {
                EmployeeId = employeeId,
                FirstName = "first name",
                LastName = "last name",
                Email = "test@gmail.com",
                JobRoleId = 1,
                InterviewRoundId = 1,
                JobRole = new JobRole { JobRoleId = 1, JobRoleName = "Developer" },
                InterviewRound = new InterviewRounds { InterviewRoundId = 1, InterviewRoundName = "First Round" }
            };

            mockAdminRepository.Setup(repo => repo.GetEmployeeById(employeeId))
                                .Returns(mockEmployee);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var actual = target.GetEmployeeById(employeeId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(employeeId, actual.Data.EmployeeId);
            Assert.Equal(mockEmployee.FirstName, actual.Data.FirstName);
            Assert.Equal(mockEmployee.LastName, actual.Data.LastName);
            Assert.Equal(mockEmployee.Email, actual.Data.Email);
            Assert.Equal(mockEmployee.JobRoleId, actual.Data.JobRoleId);
            Assert.Equal(mockEmployee.InterviewRoundId, actual.Data.InterviewRoundId);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetEmployeeById_JobRoleIdNull_ReturnsEmployeeDto()
        {
            // Arrange
            int employeeId = 1;
            var mockEmployee = new Employees
            {
                EmployeeId = employeeId,
                FirstName = "first name",
                LastName = "last name",
                Email = "test@gmail.com",
                JobRoleId = null,
                InterviewRoundId = 1,
                JobRole = new JobRole { JobRoleId = 1, JobRoleName = "Developer" },
                InterviewRound = new InterviewRounds { InterviewRoundId = 1, InterviewRoundName = "First Round" }
            };

            mockAdminRepository.Setup(repo => repo.GetEmployeeById(employeeId))
                                .Returns(mockEmployee);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var actual = target.GetEmployeeById(employeeId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(employeeId, actual.Data.EmployeeId);
            Assert.Equal(mockEmployee.FirstName, actual.Data.FirstName);
            Assert.Equal(mockEmployee.LastName, actual.Data.LastName);
            Assert.Equal(mockEmployee.Email, actual.Data.Email);
            Assert.Equal(mockEmployee.JobRoleId, actual.Data.JobRoleId);
            }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetEmployeeById_EmployeeNull_ReturnsFalse()
        {
            // Arrange
            int employeeId = 1;
            var mockEmployee = new Employees();

            mockAdminRepository.Setup(repo => repo.GetEmployeeById(employeeId))
                                .Returns<Employees>(null);

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var actual = target.GetEmployeeById(employeeId);

            // Assert
            Assert.False(actual.Success);

        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]

        public void GetEmployeeById_Exception_ReturnsErrorResponse()
        {
            // Arrange
            int id = 1;
            mockAdminRepository.Setup(repo => repo.GetEmployeeById(id))
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.GetEmployeeById(id);

            // Assert
            Assert.False(result.Success); 
            Assert.Equal("Simulated repository exception", result.Message); 
            Assert.Null(result.Data); 
        }





        // ----------------------------------------------------------
        // GetAllEmployees
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllEmployees_ReturnsEmployees_WhenEmployeeExist_SearchIsNull_IsRecruiterTrue()
        {
            // Arrange
            string sortOrder = "asc";
            var users = new List<Employees>
            {
                new Employees
                {
                    EmployeeId = 1,
                    FirstName = "First name 1",
                    LastName = "Last name 1",
                    Email = "test@example.com",
                    IsAdmin = true,
                    IsRecruiter = false,
                    IsActive = true,
                    JobRoleId = 1,
                    JobRole = new JobRole{ JobRoleId = 1, JobRoleName = "Job role 1" },
                    InterviewRoundId = 1,
                    InterviewRound = new InterviewRounds{ InterviewRoundId = 1, InterviewRoundName = "Interview round  1" }
                },

                new Employees
                {
                    EmployeeId = 2,
                    FirstName = "First name 2",
                    LastName = "Last name 2",
                    Email = "test1@example.com",
                    IsAdmin = true,
                    IsRecruiter = false,
                    IsActive = true,
                    JobRoleId = 2,
                    JobRole = new JobRole{ JobRoleId = 2, JobRoleName = "Job role 2" },
                    InterviewRoundId = 2,
                    InterviewRound = new InterviewRounds{ InterviewRoundId = 2, InterviewRoundName = "Interview round  2" }
                }
            };
            int page = 1;
            int pageSize = 2;

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            mockAdminRepository.Setup(r => r.GetAllEmployees(page, pageSize, null, sortOrder)).Returns(users);

            // Act
            var actual = target.GetAllEmployees(page, pageSize, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal("Retrived emplopyees successfully!", actual.Message);
            Assert.Equal(users.Count, actual.Data.Count());
            mockAdminRepository.Verify(r => r.GetAllEmployees(page, pageSize, null, sortOrder), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllEmployees_ReturnsEmployees_WhenEmployeeExist_SearchIsNull_IsRecruiterFalse()
        {
            // Arrange
            string sortOrder = "asc";
            var users = new List<Employees>
            {
                new Employees
                {
                    EmployeeId = 1,
                    FirstName = "First name 1",
                    LastName = "Last name 1",
                    Email = "test@example.com",
                    IsRecruiter = true
                },

                new Employees
                {
                    EmployeeId = 2,
                    FirstName = "First name 2",
                    LastName = "Last name 2",
                    Email = "test1@example.com",
                    IsRecruiter = true
                }
            };
            int page = 1;
            int pageSize = 2;

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            mockAdminRepository.Setup(r => r.GetAllEmployees(page, pageSize, null, sortOrder)).Returns(users);

            // Act
            var actual = target.GetAllEmployees(page, pageSize, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal("Retrived emplopyees successfully!", actual.Message);
            Assert.Equal(users.Count, actual.Data.Count());
            mockAdminRepository.Verify(r => r.GetAllEmployees(page, pageSize, null, sortOrder), Times.Once);
        }


        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllEmployees_ReturnsNoEmployees_WhenNoEmployeeExist_SearchIsNull()
        {

            // Arrange
            string sortOrder = "asc";
            int page = 1;
            int pageSize = 2;

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            mockAdminRepository.Setup(r => r.GetAllEmployees(page, pageSize, null, sortOrder)).Returns<IEnumerable<Employees>>(null);


            // Act
            var actual = target.GetAllEmployees(page, pageSize, null, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockAdminRepository.Verify(r => r.GetAllEmployees(page, pageSize, null, sortOrder), Times.Once);
        }


        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllEmployees_Exception_ReturnsErrorResponse()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string search = "test";
            string sortOrder = "asc";
            mockAdminRepository.Setup(repo => repo.GetAllEmployees(page, pageSize, search, sortOrder))
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.GetAllEmployees(page, pageSize, search, sortOrder);

            // Assert
            Assert.False(result.Success); 
            Assert.Null(result.Data);
            Assert.Equal("Simulated repository exception", result.Message); 
        }





        // -------------
        // TotalEmployeeCount
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void TotalEmployeeCount_ReturnsEmployees_WhenSearchIsNotNull()
        {
            string search = "abc";
            var employees = new List<Employees>
            {
                new Employees
                {
                    EmployeeId = 1,
                    FirstName = "first name 1"

                },
                new Employees
                {
                    EmployeeId = 2,
                    FirstName = "first name 2"

                }
            };
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            mockAdminRepository.Setup(r => r.TotalEmployeeCount(search)).Returns(employees.Count);

            // Act
            var actual = target.TotalEmployeeCount(search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(employees.Count, actual.Data);
            mockAdminRepository.Verify(r => r.TotalEmployeeCount(search), Times.Once);
        }
      

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void AddEmployee_ReturnsEmployeeAlreadyExists_WhenUserExists()
        {
            // Arrange
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            var addEmployeeDto = new AddEmployeeDto
            {
                FirstName = "first name 1",
                Email = "existingUser@example.com",
            };
            mockAdminRepository.Setup(repo => repo.EmployeeExists(addEmployeeDto.Email)).Returns(true);

            // Act
            var result = target.AddEmployee(addEmployeeDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Interviewer already exist", result.Message);
            mockAdminRepository.Verify(repo => repo.EmployeeExists(addEmployeeDto.Email), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]

        public void AddEmployee_Exception_ReturnsErrorResponse()
        {
            // Arrange
            var register = new AddEmployeeDto
            {
                FirstName = "test",
                LastName = "test",
                JobRoleId = 1,
                InterviewRoundId = 1
            };
            mockAdminRepository.Setup(repo => repo.EmployeeExists(register.Email))
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.AddEmployee(register);

            // Assert
            Assert.False(result.Success); 
            Assert.Equal("Simulated repository exception", result.Message); 
        }


        [Fact]
        [Trait("Admin", "AdminServiceTests")]

        public void TotalEmployeeCount_Exception_ReturnsErrorResponse()
        {
            // Arrange
            string search = "test";
            mockAdminRepository.Setup(repo => repo.TotalEmployeeCount(search))
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.TotalEmployeeCount(search);

            // Assert
            Assert.False(result.Success); 
            Assert.Equal("Simulated repository exception", result.Message); 
            Assert.Equal(0, result.Data); 
        }




        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void RegisterUserService_ReturnsSuccess_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            var addEmployeeDto = new AddEmployeeDto
            {
                FirstName = "First name 1",
                LastName = "Last name 1",
                Email = "test@example.com",
                JobRoleId = 1,
                InterviewRoundId = 1,
            };

            mockAdminRepository.Setup(repo => repo.EmployeeExists(addEmployeeDto.Email)).Returns(false);
            mockAdminRepository.Setup(repo => repo.AddEmployee(It.IsAny<Employees>())).Returns(true);

            // Act
            var result = target.AddEmployee(addEmployeeDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Interviewer created successfully.", result.Message);
            mockAdminRepository.Verify(repo => repo.EmployeeExists(addEmployeeDto.Email), Times.Once);
            mockAdminRepository.Verify(repo => repo.AddEmployee(It.IsAny<Employees>()), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void RegisterUserService_ReturnsSomethingWentWrong_WhenEmployeeAdditionFails()
        {
            // Arrange
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            var addEmployeeDto = new AddEmployeeDto
            {
                FirstName = "First name 1",
                LastName = "Last name 1",
                Email = "test@example.com",
                JobRoleId = 1,
                InterviewRoundId = 1,
            };

            mockAdminRepository.Setup(repo => repo.EmployeeExists(addEmployeeDto.Email)).Returns(false);
            mockAdminRepository.Setup(repo => repo.AddEmployee(It.IsAny<Employees>())).Returns(false);

            // Act
            var result = target.AddEmployee(addEmployeeDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after sometime", result.Message);
            mockAdminRepository.Verify(repo => repo.EmployeeExists(addEmployeeDto.Email), Times.Once);
            mockAdminRepository.Verify(repo => repo.AddEmployee(It.IsAny<Employees>()), Times.Once);
        }
       
        
        
        
        
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetIsChangedPasswordById_ReturnsSuccessTrue_WhenChangePasswordIsTrue()
        {
            // Arrange
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            var employee = new Employees
            {
                EmployeeId = 1,
                FirstName = "First name 1",
                LastName = "Last name 1",
                Email = "test@example.com",
                JobRoleId = 1,
                InterviewRoundId = 1,
                ChangePassword = true
            };
            mockAdminRepository.Setup(repo => repo.GetEmployeeById(employee.EmployeeId)).Returns(employee);

            // Act
            var result = target.GetIsChangedPasswordById(employee.EmployeeId);

            // Assert
            Assert.True(result.Data);
            Assert.True(result.Success);
            Assert.Equal("ChangePassword is true", result.Message);
            mockAdminRepository.Verify(repo => repo.GetEmployeeById(employee.EmployeeId), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetIsChangedPasswordById_ReturnsSuccessTrue_WhenChangePasswordIsFalse()
        {
            // Arrange
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            var employee = new Employees
            {
                EmployeeId = 1,
                FirstName = "First name 1",
                LastName = "Last name 1",
                Email = "test@example.com",
                JobRoleId = 1,
                InterviewRoundId = 1,
                ChangePassword = false
            };
            mockAdminRepository.Setup(repo => repo.GetEmployeeById(employee.EmployeeId)).Returns(employee);

            // Act
            var result = target.GetIsChangedPasswordById(employee.EmployeeId);

            // Assert
            Assert.False(result.Data);
            Assert.True(result.Success);
            Assert.Equal("ChangePassword is false", result.Message);
            mockAdminRepository.Verify(repo => repo.GetEmployeeById(employee.EmployeeId), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetIsChangedPasswordById_ReturnsSuccessFalseAndSomethingWentWrong_WhenNoEmployeeExistsById()
        {
            // Arrange
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);
            var employee = new Employees();
            employee = null;
            mockAdminRepository.Setup(repo => repo.GetEmployeeById(1)).Returns(employee);

            // Act
            var result = target.GetIsChangedPasswordById(1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong,try after sometime", result.Message);
            mockAdminRepository.Verify(repo => repo.GetEmployeeById(1), Times.Once);
        }


        [Fact]
        [Trait("Admin", "AdminServiceTests")]

        public void GetIsChangedPasswordById_Exception_ReturnsErrorResponse()
        {
            // Arrange
            int id = 1;
            mockAdminRepository.Setup(repo => repo.GetEmployeeById(id))
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.GetIsChangedPasswordById(id);

            // Assert
            Assert.False(result.Success); 
            Assert.Equal("Simulated repository exception", result.Message); 
            Assert.False(result.Data); 
        }



        // GetAllJobRoles
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllJobRoles_ReturnsErrorMessage_WhenNoJobeRolesExists()
        {
            // Arranage
            var jobRoles = new List<JobRole>();
            var response = new ServiceResponse<IEnumerable<JobRole>>()
            {
                Data = jobRoles,
                Success = false,
                Message = "No record found",
            };
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            mockAdminRepository.Setup(x => x.GetAllJobRoles()).Returns(jobRoles);

            // Act
            var actual = target.GetAllJobRoles();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(x => x.GetAllJobRoles(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllJobRoles_ReturnsFalse_WhenJobeRolesAreNull()
        {
            // Arrange
            IEnumerable<JobRole> jobRoles = null;

            var response = new ServiceResponse<IEnumerable<JobRoleDto>>()
            {
                Success = false,
                Message = "No record found"
            };
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            mockAdminRepository.Setup(c => c.GetAllJobRoles()).Returns(jobRoles);

            // Act
            var actual = target.GetAllJobRoles();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Data, actual.Data);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllJobRoles(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllJobRoles_ReturnsTrue_WhenJobRolesExist()
        {
            // Arrange
            IEnumerable<JobRole> jobRoles = new List<JobRole>()
            {
                new JobRole() { JobRoleId=1, JobRoleName="jobrole1" },
                new JobRole() { JobRoleId=2, JobRoleName="jobrole2" },
            };

            var response = new ServiceResponse<IEnumerable<JobRoleDto>>()
            {
                Success = true,
                Message = "Success"
            };

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            mockAdminRepository.Setup(c => c.GetAllJobRoles()).Returns(jobRoles);

            // Act
            var actual = target.GetAllJobRoles();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllJobRoles(), Times.Once);
        }


        [Fact]
        [Trait("Admin", "AdminServiceTests")]

        public void GetAllJobRoles_Exception_ReturnsErrorResponse()
        {
            
            mockAdminRepository.Setup(repo => repo.GetAllJobRoles())
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.GetAllJobRoles();

            // Assert
            Assert.False(result.Success); 
            Assert.Equal("Simulated repository exception", result.Message); 
            Assert.Null(result.Data);
        }




        // GetAllInterviewRounds
        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllInterviewRounds_ReturnsErrorMessage_WhenNoInterviewRoundsExists()
        {
            // Arranage
            var interviewRounds = new List<InterviewRounds>();
            var response = new ServiceResponse<IEnumerable<InterviewRounds>>()
            {
                Data = interviewRounds,
                Success = false,
                Message = "No record found",
            };
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            mockAdminRepository.Setup(x => x.GetAllInterviewRounds()).Returns(interviewRounds);

            // Act
            var actual = target.GetAllInterviewRounds();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(x => x.GetAllInterviewRounds(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllInterviewRounds_ReturnsFalse_WhenInterviewRoundsAreNull()
        {
            // Arrange
            IEnumerable<InterviewRounds> interviewRounds = null;

            var response = new ServiceResponse<IEnumerable<InterviewRoundsDto>>()
            {
                Success = false,
                Message = "No record found"
            };
            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            mockAdminRepository.Setup(c => c.GetAllInterviewRounds()).Returns(interviewRounds);

            // Act
            var actual = target.GetAllInterviewRounds();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Data, actual.Data);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllInterviewRounds(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]
        public void GetAllInterviewRounds_ReturnsTrue_WhenJobRolesExist()
        {
            // Arrange
            IEnumerable<InterviewRounds> jobRoles = new List<InterviewRounds>()
            {
                new InterviewRounds() { InterviewRoundId=1, InterviewRoundName="interviewround1" },
                new InterviewRounds() { InterviewRoundId=2, InterviewRoundName="interviewround2" },
            };

            var response = new ServiceResponse<IEnumerable<InterviewRoundsDto>>()
            {
                Success = true,
                Message = "Success"
            };

            var target = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            mockAdminRepository.Setup(c => c.GetAllInterviewRounds()).Returns(jobRoles);

            // Act
            var actual = target.GetAllInterviewRounds();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Success, actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAdminRepository.Verify(c => c.GetAllInterviewRounds(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminServiceTests")]

        public void GetAllInterviewRounds_Exception_ReturnsErrorResponse()
        {

            mockAdminRepository.Setup(repo => repo.GetAllInterviewRounds())
                               .Throws(new Exception("Simulated repository exception"));

            var service = new AdminService(mockAdminRepository.Object, mockPasswordService.Object);

            // Act
            var result = service.GetAllInterviewRounds();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Simulated repository exception", result.Message);
            Assert.Null(result.Data);
        }




        public void Dispose()
        {
            mockAdminRepository.VerifyAll();
        }
    }
}
