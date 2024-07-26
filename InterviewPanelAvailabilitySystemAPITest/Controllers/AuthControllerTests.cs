using InterviewPanelAvailabilitySystemAPI.Controllers;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
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
    public class AuthControllerTests  : IDisposable
    {
        private readonly Mock<IAuthService> mockAuthService;

        public AuthControllerTests()
        {
            mockAuthService = new Mock<IAuthService>();
        }

        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChangePassword_ReturnsOkResponse_WhenChangePasswordSuccessfully()
        {
            var changePasswordDto = new ChangePasswordDto
            {
                Email = "email@test.com",
                OldPassword = "TestOldPassword@123",
                NewPassword = "TestNewPassword@123",
                NewConfirmPassword = "TestNewPassword@123"
            };
            var response = new ServiceResponse<string>
            {
                Success = true,
                Message = "Password changed successfully."
            };
            
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.ChangePassword(It.IsAny<ChangePasswordDto>())).Returns(response);

            //Act
            var actual = target.ChangePassword(changePasswordDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.ChangePassword(It.IsAny<ChangePasswordDto>()), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChangePassword_ReturnsBadRequest_WhenPasswordIsNotChanged()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                Email = "email@test.com",
                OldPassword = "TestOldPassword@123",
                NewPassword = "TestNewPassword@123",
                NewConfirmPassword = "TestNewPassword@123"
            };
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.ChangePassword(It.IsAny<ChangePasswordDto>())).Returns(response);

            // Act
            var actual = target.ChangePassword(changePasswordDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.ChangePassword(It.IsAny<ChangePasswordDto>()), Times.Once);
        }
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChangePassword_ReturnsException()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto
            {
                Email = "email@test.com",
                OldPassword = "TestOldPassword@123",
                NewPassword = "TestNewPassword@123",
                NewConfirmPassword = "TestNewPassword@123"
            };
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.ChangePassword(It.IsAny<ChangePasswordDto>())).Throws(new Exception());

            // Act
            var actual = target.ChangePassword(changePasswordDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
        }
        [Theory]
        [InlineData("Invalid user email or password!")]
        [InlineData("Something went wrong, please try after some time")]
        [Trait("Auth", "AuthControllerTests")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void Login_ReturnsException()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                

            };
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Throws(new Exception());

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
        }

        public void Dispose()
        {
            mockAuthService.VerifyAll();
        }
    }
}
