using InterviewPanelAvailabilitySystemMVC.Controllers;
using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemMVCTests.ControllerTests
{
    public class AuthControllerTests : IDisposable
    {
        private readonly Mock<IHttpClientService> _mockHttpClientService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IJwtTokenHandler> _mockTokenHandler;
        private readonly Mock<HttpContext> _mockHttpContext;

        public AuthControllerTests()
        {
            _mockHttpClientService = new Mock<IHttpClientService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockTokenHandler = new Mock<IJwtTokenHandler>();
            _mockHttpContext = new Mock<HttpContext>();
        }
        // ChnagePassword
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChnagePassword_ReturnsViews()
        {
            // Arrange
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };
            //Act
            var result = target.ChangePassword() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChangePassword_ModelIsInvalid()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel { NewPassword = "Password@123" };
            _mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("OldPassword", "Old Password is required");

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(changePasswordViewModel, actual.Model);
            _mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChnagePassword_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel
            {
                OldPassword = "Oldpassword@123",
                NewPassword = "Password@123",
                NewConfirmPassword = "Password@123"
            };
            _mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            _mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            _mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            _mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);

        }

        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChnagePassword_Success_RedirectToAction()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel
            {
                OldPassword = "Oldpassword@123",
                NewPassword = "Password@123",
                NewConfirmPassword = "Password@123"
            };

            _mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,

            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            _mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
             .Returns(expectedResponse);

            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Delete("jwtToken"));
            var mockHttpResponse = new Mock<HttpResponse>();
            _mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("LoginUser", actual.ActionName);
            Assert.Equal("Auth", actual.ControllerName);
            Assert.True(target.ModelState.IsValid);
            mockResponseCookie.Verify(c => c.Delete("jwtToken"), Times.Once);
        }

        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChnagePassword_RedirectToAction_WhenBadRequest_WhenResponseIsNull()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel
            {
                OldPassword = "Oldpassword@123",
                NewPassword = "Password@123",
                NewConfirmPassword = "Password@123"
            };
            _mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            _mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ChangePassword(changePasswordViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong, please try after sometime", target.TempData["ErrorMessage"]);
            _mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            _mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        // -------Catch block handle case

        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void ChangePassword_ExceptionOccurs_CatchesAndSetsErrorMessage()
        {
            // Arrange
            var changePasswordViewModel = new ChangePasswordViewModel
            {
                OldPassword = "Oldpassword@123",
                NewPassword = "Password@123",
                NewConfirmPassword = "Password@123"
            };

            _mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            _mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()))
                                 .Throws(new Exception("Fake Exception"));
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };

            // Act
            var result = target.ChangePassword(changePasswordViewModel) as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(changePasswordViewModel, result.Model);
            Assert.True(target.TempData.ContainsKey("ErrorMessage")); // Check TempData for the error message
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            Assert.True(target.ModelState.IsValid);
            _mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            _mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), changePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        // LoginUser (Get)
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void LoginUser_ReturnsViews()
        {
            // Arrange
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };
            // Act
            var result = target.LoginUser() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        // Login Post
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void Login_ModelIsInvalid()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123" };
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("Username", "Email is required");
            //Act
            var actual = target.LoginUser(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(loginViewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
        }


        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void Login_ReturnView_WhenBadRequest()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", Username = "loginid" };

            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            _mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(_mockHttpClientService.Object, _mockConfiguration.Object, _mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.LoginUser(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            _mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Login_ReturnView_WhenStatusCodeIsSuccess_redirecttoChangePassword()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", Username = "loginid" };
            var employeeViewModel = new EmployeeViewModel
            { FirstName="FirstName" };
            var mockToken = "mockToken";
            var mockEmployeeId = "1";
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            mockResponseCookie.Setup(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["Endpoint:CivicaContactApi"]).Returns("fakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Data = mockToken,
            };
            var expectedServiceResponse1 = new ServiceResponse<EmployeeViewModel>
            {
                Success = true,
                Data = employeeViewModel,
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var expectedResponse1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse1))
            };

            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
                                 .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<EmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                                .Returns(expectedResponse1);
            var claims = new[]
            {
                new Claim("EmployeeId", mockEmployeeId)
            };
            var jwtToken = new JwtSecurityToken(claims: claims);
            mockTokenHandler.Setup(t => t.ReadJwtToken(mockToken)).Returns(jwtToken);


            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.LoginUser(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            mockResponseCookie.Verify(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()), Times.Once);
            mockResponseCookie.Verify(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("ChangePassword", result.ActionName);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Login_ReturnView_WhenStatusCodeIsSuccess_redirecttoIndex()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", Username = "loginid"};
            var employeeViewModel = new EmployeeViewModel
            { FirstName = "FirstName",ChangePassword=true };
            var mockToken = "mockToken";
            var mockEmployeeId = "1";
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            mockResponseCookie.Setup(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["Endpoint:CivicaContactApi"]).Returns("fakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Data = mockToken,
            };
            var expectedServiceResponse1 = new ServiceResponse<EmployeeViewModel>
            {
                Success = true,
                Data = employeeViewModel,
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var expectedResponse1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse1))
            };

            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
                                 .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<EmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                                .Returns(expectedResponse1);
            var claims = new[]
            {
                new Claim("EmployeeId", mockEmployeeId)
            };
            var jwtToken = new JwtSecurityToken(claims: claims);
            mockTokenHandler.Setup(t => t.ReadJwtToken(mockToken)).Returns(jwtToken);


            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.LoginUser(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            mockResponseCookie.Verify(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()), Times.Once);
            mockResponseCookie.Verify(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", result.ActionName);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Login_ReturnException()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", Username = "loginid" };
            var employeeViewModel = new EmployeeViewModel
            { FirstName = "FirstName" };
            var mockToken = "mockToken";
            var mockEmployeeId = "1";
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            mockResponseCookie.Setup(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["Endpoint:CivicaContactApi"]).Returns("fakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Data = mockToken,
            };
            var expectedServiceResponse1 = new ServiceResponse<EmployeeViewModel>
            {
                Success = true,
                Data = employeeViewModel,
            };
            var expectedResponse1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse1))
            };

            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
                                 .Throws(new Exception());
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<EmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                                .Returns(expectedResponse1);
            var claims = new[]
            {
                new Claim("EmployeeId", mockEmployeeId)
            };
            var jwtToken = new JwtSecurityToken(claims: claims);
            mockTokenHandler.Setup(t => t.ReadJwtToken(mockToken)).Returns(jwtToken);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);


            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.LoginUser(loginViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(target.ModelState.IsValid);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Login_ReturnHttpRequestException()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", Username = "loginid" };
            var employeeViewModel = new EmployeeViewModel
            { FirstName = "FirstName" };
            var mockToken = "mockToken";
            var mockEmployeeId = "1";
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            mockResponseCookie.Setup(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["Endpoint:CivicaContactApi"]).Returns("fakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Data = mockToken,
            };
            var expectedServiceResponse1 = new ServiceResponse<EmployeeViewModel>
            {
                Success = true,
                Data = employeeViewModel,
            };

            var expectedResponse1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse1))
            };

            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
                                 .Throws(new HttpRequestException());
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<EmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                                .Returns(expectedResponse1);
            var claims = new[]
            {
                new Claim("EmployeeId", mockEmployeeId)
            };
            var jwtToken = new JwtSecurityToken(claims: claims);
            mockTokenHandler.Setup(t => t.ReadJwtToken(mockToken)).Returns(jwtToken);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData=tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.LoginUser(loginViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("An error occurred while communicating with the server. Please try again later.", target.TempData["ErrorMessage"]);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Login_ReturnView_WhenStatusCodeIsSuccess_ReturnException()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", Username = "loginid" };
            var employeeViewModel = new EmployeeViewModel
            { FirstName = "FirstName" };
            var mockToken = "mockToken";
            var mockEmployeeId = "1";
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            mockResponseCookie.Setup(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["Endpoint:CivicaContactApi"]).Returns("fakeEndPoint");
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Data = mockToken,
            };
            var expectedServiceResponse1 = new ServiceResponse<EmployeeViewModel>
            {
                Success = true,
                Data = employeeViewModel,
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
           

            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
                                 .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<EmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                                .Throws(new Exception());
            var claims = new[]
            {
                new Claim("EmployeeId", mockEmployeeId)
            };
            var jwtToken = new JwtSecurityToken(claims: claims);
            mockTokenHandler.Setup(t => t.ReadJwtToken(mockToken)).Returns(jwtToken);


            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,

                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var result = target.LoginUser(loginViewModel) as RedirectToActionResult;

            // Assert
            mockResponseCookie.Verify(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()), Times.Once);
            mockResponseCookie.Verify(c => c.Append("employeeId", mockEmployeeId, It.IsAny<CookieOptions>()), Times.Once);
            Assert.True(target.ModelState.IsValid);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);

        }
       

        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void Login_RedirectToAction_WhenBadRequest_WhenResponseIsNull()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", Username = "loginid" }; var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var mockToken = new Mock<IJwtTokenHandler>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockToken.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var actual = target.LoginUser(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong, please try after sometime", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        //Logout
        [Fact]
        [Trait("Auth", "AuthControllerTests")]
        public void Logout_RedirectsToAction_WhenLogoutSuccessful()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new DefaultHttpContext();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("endPoint");

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext,
                }
            };

            // Act
            var actual = target.LogoutUser() as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        public void Dispose()
        {
            _mockHttpClientService.VerifyAll();
            _mockConfiguration.VerifyAll();
            _mockTokenHandler.VerifyAll();
            _mockHttpContext.VerifyAll();
        }
    }
}
