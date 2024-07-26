using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using InterviewPanelAvailabilitySystemAPI.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemAPITest.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly Mock<IAuthRepository> mockAuthRepository;
        private readonly Mock<IPasswordService> mockConfiguration;

        public AuthServiceTests()
        {
            mockAuthRepository = new Mock<IAuthRepository>();
            mockConfiguration = new Mock<IPasswordService>();
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsSomethingWentWrong_WhenLoginDtoIsNull()
        {
            //Arrange
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after some time", result.Message);

        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsNull()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username@test.com"
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns<Employees>(null);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid user email or password!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);


        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenPasswordIsWrong()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username@test.com",
                Password = "password"
            };
            var user = new Employees
            {
                EmployeeId = 1,
                Email = "abc@gmail.com",
                IsActive = true,
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid user email or password!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsActiveFalse()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username@test.com",
                Password = "password"
            };
            var user = new Employees
            {
                EmployeeId = 1,
                Email = "abc@gmail.com",
                IsActive = false,
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid user email or password!", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
        }     
        
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ThrowsException()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username@test.com",
                Password = "password"
            };

            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Throws(new Exception("Simulated exception")); ;

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Simulated exception", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void LoginUserService_ReturnsResponse_WhenLoginIsSuccessful()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                Username = "username@test.com",
                Password = "Password@123"
            };
            var user = new Employees
            {
                EmployeeId = 1,
                Email = "abc@gmail.com",
                IsActive = true
                
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.Username)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockConfiguration.Setup(repo => repo.CreateToken(user)).Returns("");

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.Username), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);
            mockConfiguration.Verify(repo => repo.CreateToken(user), Times.Once);
        }
        // ChangePassword
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenExistingUerIsNull()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                Email = "Test@test.com",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Invalid email or password"
            };

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.Email)).Returns<Employees>(null);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.Email), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenNewAndOldPasswordIsSame()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                Email = "Test@test.com",
                OldPassword = "NewTest@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Old password and new password can not be same."
            };

            var user = new Employees()
            {
                EmployeeId = 2,
                FirstName = "test",
                Email = changePasswordDto.Email,
            };

            mockConfiguration.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.Email)).Returns(user);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.Email), Times.Once);
            mockConfiguration.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenVerifyPasswordHashFails()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                Email = "Test@test.com",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Old password is incorrect."
            };

            var user = new Employees()
            {
                EmployeeId = 2,
                FirstName = "test",
                Email = changePasswordDto.Email,
            };

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.Email)).Returns(user);
            mockConfiguration.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.Email), Times.Once);
            mockConfiguration.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        }


         [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ThrowsException()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                Email = "Test@test.com",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.Email)).Throws(new Exception("Simulated exception")); ;

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.Null(actual.Data);
            Assert.False(actual.Success);
            Assert.Equal("Simulated exception", actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.Email), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenUpdationFails()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                Email = "Test@test.com",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Something went wrong, please try after sometime"
            };

            var user = new Employees()
            {
                EmployeeId = 2,
                FirstName = "test",
                Email = changePasswordDto.Email,
            };
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.Email)).Returns(user);
            mockConfiguration.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(p => p.UpdateUser(It.IsAny<Employees>())).Returns(false);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.Email), Times.Once);
            mockConfiguration.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockAuthRepository.Verify(p => p.UpdateUser(It.IsAny<Employees>()), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsSuccessMessage_WhenUpdatedSuccessfully()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                Email = "Test@test.com",
                OldPassword = "Test@123",
                NewPassword = "NewTest@123",
                NewConfirmPassword = "NewTest@123"
            };

            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = true,
                Message = "Successfully changed password, Please signin!"
            };

            var user = new Employees()
            {
                EmployeeId = 2,
                FirstName = "test",
                Email = changePasswordDto.Email,
            };
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.Email)).Returns(user);
            mockConfiguration.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockAuthRepository.Setup(p => p.UpdateUser(It.IsAny<Employees>())).Returns(true);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.Email), Times.Once);
            mockConfiguration.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockAuthRepository.Verify(p => p.UpdateUser(It.IsAny<Employees>()), Times.Once);
        }
        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenPasswordStrengthIsNotProper()
        {
            var changePasswordDto = new ChangePasswordDto()
            {
                Email = "Test@test.com",
                OldPassword = "NewTest@123",
                NewPassword = "test",
                NewConfirmPassword = "test"
            };
            string message = "Mininum password length should be 8\r\nPassword should be alphanumeric\r\nPassword should contain special characters\r\n";
            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = message
            };
            var user = new Employees()
            {
                EmployeeId = 2,
                FirstName = "test",
                Email = changePasswordDto.Email,
            };
            mockConfiguration.Setup(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            mockConfiguration.Setup(x => x.CheckPasswordStrength(changePasswordDto.NewPassword)).Returns(message);
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            mockAuthRepository.Setup(c => c.ValidateUser(changePasswordDto.Email)).Returns(user);

            //Act
            var actual = target.ChangePassword(changePasswordDto);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(changePasswordDto.Email), Times.Once);
            mockConfiguration.Verify(x => x.VerifyPasswordHash(changePasswordDto.OldPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            mockConfiguration.Verify(x => x.CheckPasswordStrength(changePasswordDto.NewPassword), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthServiceTests")]
        public void ChangePassword_ReturnsErrorMessage_WhenDtoIsNull()
        {
            // Arrange
            var changePasswordDto = new ChangePasswordDto() { };
            changePasswordDto = null;
            var response = new ServiceResponse<ChangePasswordDto>()
            {
                Data = changePasswordDto,
                Success = false,
                Message = "Something went wrong, please try after sometime"
            };
            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var actual = target.ChangePassword(changePasswordDto);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal(response.Message, actual.Message);
        }
        public void Dispose()
        {
            mockAuthRepository.VerifyAll();
            mockConfiguration.VerifyAll();
        }

    }
}
