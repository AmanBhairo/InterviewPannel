using InterviewPanelAvailabilitySystemAPI.Controllers;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Controllers
{
    public class AdminControllerTests : IDisposable
    {

        private readonly Mock<IAdminService> mockAdminService;

        public AdminControllerTests()
        {
            mockAdminService = new Mock<IAdminService>();
        }



        //RemoveEmployee
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void RemoveEmployee_ReturnsNotFound()
        {
            var employeeId = 1;
            var book = new UpdateEmployeeDtos
            {
                FirstName = "test",
                LastName = "test last"
            };

            var response = new ServiceResponse<string>
            {
                Success = false,
            };

            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.DeleteEmployee(employeeId)).Returns(response);

            //Act
            var actual = target.RemoveEmployee(employeeId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.DeleteEmployee(employeeId), Times.Once);
        }



        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void RemoveEmployee_OkResponse()
        {
            var employeeId = 1;
            var employee = new UpdateEmployeeDtos
            {
                FirstName = "test",
                LastName = "Last test",
            };

            var response = new ServiceResponse<string>
            {
                Success = true,

            };
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.DeleteEmployee(employeeId)).Returns(response);

            //Act
            var actual = target.RemoveEmployee(employeeId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.DeleteEmployee(employeeId), Times.Once);
        }

        //3rd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void RemoveEmployee_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
            var employee = new UpdateEmployeeDtos
            {
                FirstName = "test",
                LastName = "Last test",
            };
            var exceptionMessage = "Simulated exception message";

            mockAdminService.Setup(x => x.DeleteEmployee(id))
                .Throws(new Exception(exceptionMessage));

            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.RemoveEmployee(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); 
            Assert.Equal(exceptionMessage, result.Value); 
        }





        //UpdateEmployee
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void UpdateEmployee_ReturnBadRequest_WhenSuccessIsFalse()
        {
            // Arrange
            var employees = new Employees() { FirstName = "Category 1", LastName = "Description 1" };
            var employeeDto = new UpdateEmployeeDtos { FirstName = employees.FirstName, LastName = employees.LastName };
            var response = new ServiceResponse<string>()
            {
                Data = { },
                Success = false,
                Message = "",
            };
            mockAdminService.Setup(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDtos>())).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.Edit(employeeDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            mockAdminService.Verify(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDtos>()), Times.Once);
        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void UpdateEmployee_ReturnOkResponse_WhenSuccessIsTrue()
        {
            // Arrange
            var employee = new Employees() { FirstName = "test 1", LastName = "lastname 1" };
            var employeeDtos = new UpdateEmployeeDtos { FirstName = employee.FirstName, LastName = employee.LastName };
            var response = new ServiceResponse<string>()
            {
                Data = { },
                Success = true,
                Message = "",
            };
            mockAdminService.Setup(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDtos>())).Returns(response);

            var target = new AdminController(mockAdminService.Object);

            // Act
            var actual = target.Edit(employeeDtos) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            mockAdminService.Verify(c => c.UpdateEmployee(It.IsAny<UpdateEmployeeDtos>()), Times.Once);

        }


        //3rd case
        [Fact]
        public void Edit_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var updateEmployeeDto = new UpdateEmployeeDtos
            {
                EmployeeId = 1,
                FirstName = "Test",
                LastName = "test last",
                Email = "test@gmail.com",
                JobRoleId = 2,
                InterviewRoundId = 3
            };

            var exceptionMessage = "Simulated exception message";
            mockAdminService.Setup(x => x.UpdateEmployee(It.IsAny<UpdateEmployeeDtos>()))
                .Throws(new Exception(exceptionMessage));

            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.Edit(updateEmployeeDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); 
            Assert.Equal(exceptionMessage, result.Value); 
        }


        //GetEmployeeById
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetEmployeeById_ExistingEmployee_ReturnsOk()
        {
            // Arrange
            int employeeId = 1;
            var successResponse = new ServiceResponse<EmployeesDto>
            {
                Success = true,
                Data = new EmployeesDto { EmployeeId = employeeId, FirstName = "John", LastName = "Doe" }
            };

            mockAdminService.Setup(service => service.GetEmployeeById(employeeId)).Returns(successResponse);
            var mockController = new AdminController(mockAdminService.Object);

            // Act
            var result = mockController.GetEmployeeById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ServiceResponse<EmployeesDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(employeeId, response.Data.EmployeeId);
        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetEmployeeById_NonExistingEmployee_ReturnsNotFound()
        {
            // Arrange
            int employeeId = 999;
            var errorResponse = new ServiceResponse<EmployeesDto>
            {
                Success = false,
                Message = "Employee not found."
            };

            mockAdminService.Setup(service => service.GetEmployeeById(employeeId)).Returns(errorResponse);
            var mockController = new AdminController(mockAdminService.Object);

            // Act
            var result = mockController.GetEmployeeById(employeeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ServiceResponse<EmployeesDto>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Employee not found.", response.Message);
        }


        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetEmployeeById_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
           
            var exceptionMessage = "Simulated exception message";

            mockAdminService.Setup(x => x.GetEmployeeById(id))
                .Throws(new Exception(exceptionMessage));

            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.GetEmployeeById(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(exceptionMessage, result.Value);
        }



        // ----------------------------------------------------------
        // GetAllEmployees
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllEmployees_ReturnsOkWithEmployees_SearchIsNull()
        {
            //Arrange
            var employees = new List<Employees>
            {
               new Employees{ EmployeeId=1, FirstName="First name 1", LastName = "Last name 1"},
               new Employees{ EmployeeId=2, FirstName="First name 2", LastName = "Last name 2"},
            };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<EmployeesDto>>
            {
                Success = true,
                Data = employees.Select(c => new EmployeesDto { EmployeeId = c.EmployeeId, FirstName = c.FirstName, LastName = c.LastName })
            };

            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllEmployees(page, pageSize, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetAllEmployees(null, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllEmployees(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllEmployees_ReturnsOkWithEmployees_SearchIsNotNull()
        {
            //Arrange
            var employees = new List<Employees>
            {
               new Employees{ EmployeeId=1, FirstName="First name 1", LastName = "Last name 1"},
               new Employees{ EmployeeId=2, FirstName="First name 2", LastName = "Last name 2"},
            };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "C";
            var response = new ServiceResponse<IEnumerable<EmployeesDto>>
            {
                Success = true,
                Data = employees.Select(c => new EmployeesDto { EmployeeId = c.EmployeeId, FirstName = c.FirstName, LastName = c.LastName })
            };

            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllEmployees(page, pageSize, search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetAllEmployees(search, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllEmployees(page, pageSize, search, sortOrder), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllEmployees_ReturnsNotFound_WhenSearchIsNull()
        {
            //Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<EmployeesDto>>
            {
                Success = false,
                Data = null
            };

            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllEmployees(page, pageSize, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetAllEmployees(null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllEmployees(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllEmployees_ReturnsNotFound_WhenSearchIsNotNull()
        {
            //Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "C";

            var response = new ServiceResponse<IEnumerable<EmployeesDto>>
            {
                Success = false,
                Data = null
            };

            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllEmployees(page, pageSize, search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetAllEmployees(search, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllEmployees(page, pageSize, search, sortOrder), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllEmployees_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            string search = "test";
            int page = 1;
            int pageSize = 4;
            string sortOrder = "asc";
            var exceptionMessage = "Simulated exception message";

            mockAdminService.Setup(x => x.GetAllEmployees(page, pageSize, search, sortOrder))
                .Throws(new Exception(exceptionMessage));
            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.GetAllEmployees(search, page, pageSize, sortOrder) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); 
            Assert.Equal(exceptionMessage, result.Value); 
        }


        // AddEmployee
        [Theory]
        [InlineData("Interviewer already exist")]
        [InlineData("Something went wrong, please try after sometime.")]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        [Trait("Admin", "AdminControllerTests")]
        public void AddEmployee_ReturnsBadRequest_WhenEmployeeAdditionFails(string message)
        {
            // Arrange
            var addEmployeeDto = new AddEmployeeDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message
            };
            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(service => service.AddEmployee(addEmployeeDto)).Returns(expectedServiceResponse);


            // Act
            var actual = target.AddEmployee(addEmployeeDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAdminService.Verify(service => service.AddEmployee(addEmployeeDto), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddEmployee_ReturnsOk_WhenRegistrationSuccess()
        {
            // Arrange
            var addEmployeeDto = new AddEmployeeDto();
            var message = "Interviewer created successfully.";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = message

            };
            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(service => service.AddEmployee(addEmployeeDto)).Returns(expectedServiceResponse);


            // Act
            var actual = target.AddEmployee(addEmployeeDto) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAdminService.Verify(service => service.AddEmployee(addEmployeeDto), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddEmployees_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var addEmployeeDto = new AddEmployeeDto();

            var exceptionMessage = "Simulated exception message";

            mockAdminService.Setup(x => x.AddEmployee(addEmployeeDto))
                .Throws(new Exception(exceptionMessage));
            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.AddEmployee(addEmployeeDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(exceptionMessage, result.Value);
        }



        // GetTotalEmployeeCount
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetTotalEmployeeCount_ReturnsOkWithEmpoyees_WhenSearchIsNull()
        {
            //Arrange
            var employees = new List<Employees>
            {
               new Employees{ EmployeeId=1, FirstName="First name 1", LastName = "Last name 1"},
               new Employees{ EmployeeId=2, FirstName="First name 2", LastName = "Last name 2"},
            };
            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = employees.Count
            };

            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.TotalEmployeeCount(null)).Returns(response);

            //Act
            var actual = target.GetTotalEmployeeCount(null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockAdminService.Verify(c => c.TotalEmployeeCount(null), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetTotalEmployeeCount_ReturnsOkWithEmployees_WhenSearchIsNotNull()
        {
            //Arrange
            var employees = new List<Employees>
            {
               new Employees{ EmployeeId=1, FirstName="First name 1", LastName = "Last name 1"},
               new Employees{ EmployeeId=2, FirstName="First name 2", LastName = "Last name 2"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = employees.Count
            };

            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.TotalEmployeeCount("n")).Returns(response);

            //Act
            var actual = target.GetTotalEmployeeCount("n") as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockAdminService.Verify(c => c.TotalEmployeeCount("n"), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetTotalEmployeeCount_ReturnsNotFound_SearchIsNotNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var search = "d";
            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.TotalEmployeeCount(search)).Returns(response);

            //Act
            var actual = target.GetTotalEmployeeCount(search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockAdminService.Verify(c => c.TotalEmployeeCount(search), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetTotalEmployeeCount_ReturnsNotFound_WhenSearchIsNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.TotalEmployeeCount(null)).Returns(response);

            //Act
            var actual = target.GetTotalEmployeeCount(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockAdminService.Verify(c => c.TotalEmployeeCount(null), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]

        public void GetTotalEmployeeCount_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            string search = "test";
            var exceptionMessage = "Simulated exception message";
            var target = new AdminController(mockAdminService.Object);

            mockAdminService.Setup(x => x.TotalEmployeeCount(search))
                .Throws(new Exception(exceptionMessage));

            // Act
            var result = target.GetTotalEmployeeCount(search) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); 
            Assert.Equal(exceptionMessage, result.Value); 
        }




        // GetAllJobRoles
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllJobRoles_ReturnsOk_WhenJobeRolesExists()
        {
            //Arrange

            var jobRoles = new List<JobRoleDto>
            {
                new JobRoleDto { JobRoleId = 1, JobRoleName = "JobRole 1" },
                new JobRoleDto { JobRoleId = 2, JobRoleName = "JobRole 2" },
            };
            var response = new ServiceResponse<IEnumerable<JobRoleDto>>
            {
                Success = true,
            };

            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllJobRoles()).Returns(response);

            //Act
            var actual = target.GetAllJobRoles() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllJobRoles(), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllJobRoles_ReturnsNotFound_WhenJobRolesDoesNotExist()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<JobRoleDto>>
            {
                Success = false,
                Data = Enumerable.Empty<JobRoleDto>()
            };
            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllJobRoles()).Returns(response);

            //Act
            var actual = target.GetAllJobRoles() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllJobRoles(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllJobRoles_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            mockAdminService.Setup(x => x.GetAllJobRoles())
                .Throws(new Exception());
            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.GetAllJobRoles() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }




        // GetAllInterviewRounds
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllInterviewRounds_ReturnsOk_WhenInterviewRoundsExists()
        {
            //Arrange

            var interviewRounds = new List<InterviewRoundsDto>
            {
                new InterviewRoundsDto { InterviewRoundId = 1, InterviewRoundName = "InterviewRound 1" },
                new InterviewRoundsDto { InterviewRoundId = 2, InterviewRoundName = "InterviewRound 2" },
            };
            var response = new ServiceResponse<IEnumerable<InterviewRoundsDto>>
            {
                Success = true,
            };

            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllInterviewRounds()).Returns(response);

            //Act
            var actual = target.GetAllInterviewRounds() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllInterviewRounds(), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllInterviewRounds_ReturnsNotFound_WhenInterviewRoundsDoesNotExist()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<InterviewRoundsDto>>
            {
                Success = false,
                Data = Enumerable.Empty<InterviewRoundsDto>()
            };
            var mockAdminService = new Mock<IAdminService>();
            var target = new AdminController(mockAdminService.Object);
            mockAdminService.Setup(c => c.GetAllInterviewRounds()).Returns(response);

            //Act
            var actual = target.GetAllInterviewRounds() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAdminService.Verify(c => c.GetAllInterviewRounds(), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetAllInterviewRounds_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            mockAdminService.Setup(x => x.GetAllInterviewRounds())
                .Throws(new Exception());
            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.GetAllInterviewRounds() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }



        // GetIsChangedPasswordById
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetIsChangedPasswordById_ExistingEmployee_ReturnsOk()
        {
            // Arrange
            int employeeId = 2;
            string message = "ChangePassword is true";
            var successResponse = new ServiceResponse<bool>
            {
                Success = true,
                Data = true,
                Message = message
            };

            mockAdminService.Setup(service => service.GetIsChangedPasswordById(employeeId)).Returns(successResponse);
            var mockController = new AdminController(mockAdminService.Object);

            // Act
            var result = mockController.GetIsChangedPasswordById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ServiceResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(message, response.Message);
            Assert.True(response.Data);
            mockAdminService.Verify(service => service.GetIsChangedPasswordById(employeeId), Times.Once);
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetIsChangedPasswordById_NoExistingEmployee_ReturnsNotFound()
        {
            // Arrange
            int employeeId = 2;
            string message = "Something went wrong,try after sometime";
            var errorResponse = new ServiceResponse<bool>
            {
                Success = false,
                Message = message
            };

            mockAdminService.Setup(service => service.GetIsChangedPasswordById(employeeId)).Returns(errorResponse);
            var mockController = new AdminController(mockAdminService.Object);

            // Act
            var result = mockController.GetIsChangedPasswordById(employeeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ServiceResponse<bool>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal(message, response.Message);
            mockAdminService.Verify(service => service.GetIsChangedPasswordById(employeeId), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void GetIsChangedPasswordById_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;

            var exceptionMessage = "Simulated exception message";

            mockAdminService.Setup(x => x.GetIsChangedPasswordById(id))
                .Throws(new Exception(exceptionMessage));

            var target = new AdminController(mockAdminService.Object);

            // Act
            var result = target.GetIsChangedPasswordById(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(exceptionMessage, result.Value);
        }




        public void Dispose()
        {
            mockAdminService.VerifyAll();
        }
    }
}
