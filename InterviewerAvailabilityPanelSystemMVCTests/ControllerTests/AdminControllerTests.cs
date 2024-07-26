using InterviewPanelAvailabilitySystemMVC.Controllers;
using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemMVCTests.ControllerTests
{
    public class AdminControllerTests : IDisposable
    {
        private readonly Mock<IHttpClientService> mockHttpClientService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<IJwtTokenHandler> mockTokenHandler;
        private readonly Mock<HttpContext> mockHttpContext;

        public AdminControllerTests()
        {
            mockHttpClientService = new Mock<IHttpClientService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpContext = new Mock<HttpContext>();
        }

        //Edit - GET METHOD
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var viewModel = new UpdateEmployeeViewModel()
            {
                EmployeeId = 1,
                FirstName = "test",
                LastName = "last test",
                Email = "test@gmail.com",
                JobRoleId = 1,
                InterviewRoundId = 1,
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var jobroles = new List<JobRoleViewModel>
            {
            new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
            new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
            };

            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };

            var interviewrounds = new List<InterviewRoundViewModel>
            {
            new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
            new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
            };

            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponse1);

            var expectedServiceResponse = new ServiceResponse<UpdateEmployeeViewModel>
            {
                Data = viewModel,
                Success = true
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ReturnsView_WhenStatusCodeIsSuccessAndJobRoleInterviewRoundAreNull()
        {
            var id = 1;
            var employee = new UpdateEmployeeViewModel()
            {
                EmployeeId = id,
                FirstName = "P1"
            };
            var jobroles = new List<JobRoleViewModel>
            {
            new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
            new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
            };

            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };

            var interviewrounds = new List<InterviewRoundViewModel>
            {
            new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
            new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
            };

            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponse1);
            var expectedServiceResponse = new ServiceResponse<UpdateEmployeeViewModel>
            {
                Data = employee,
                Success = true
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ReturnsErrorDataNull_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<UpdateEmployeeViewModel>
            {
                Message = "",
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var jobroles = new List<JobRoleViewModel>
            {
            new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
            new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
            };

            
            var interviewrounds = new List<InterviewRoundViewModel>
            {
            new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
            new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
            };

           
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            
        }


        //4th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ReturnsErrorMessageNull_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<UpdateEmployeeViewModel>
            {
                Message = null,
                Data = new UpdateEmployeeViewModel { EmployeeId = id, FirstName = "C1", LastName = "D1" },
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }


        //5th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ReturnsRedirectToAction_SomethingWentWrong()
        {
            //Arrange
            var id = 1;
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Something went wrong please try after some time.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }


        //6th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ExceptionThrown_RedirectsToHomeIndexWithErrorMessage()
        {
            // Arrange
            var id = 1;
            var exceptionMessage = "Simulated exception message";

            mockHttpClientService.Setup(x => x.GetHttpResponseMessage<UpdateEmployeeViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()))
                .Throws(new Exception(exceptionMessage));
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var result = target.Edit(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Home", result.ControllerName); 
            Assert.Equal("Index", result.ActionName); 
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
        }


        //Edit - POST METHOD
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ContactSavedSuccessfully_RedirectToAction()
        {
            //Arrange
            var id = 1;
            var viewModel = new UpdateEmployeeViewModel { EmployeeId = id, FirstName = "C1", LastName = "D1" };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Contact saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);

        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ContactFailedToSaveServiceResponseNull_RedirectToAction()
        {
            //Arrange
            var id = 1;
            var viewModel = new UpdateEmployeeViewModel { EmployeeId = id, FirstName = "C1", LastName = "D1" };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };

            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            var jobroles = new List<JobRoleViewModel>
            {
            new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
            new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
            };
            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            var interviewrounds = new List<InterviewRoundViewModel>
            {
            new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
            new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
            };

            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse1);
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.Null(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ContactFailedToSave_ReturnRedirectToActionResult()
        {
            //Arrange
            var viewModel = new UpdateEmployeeViewModel { FirstName = "C1", LastName = "D1" };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = errorMessage
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            var jobroles = new List<JobRoleViewModel>
            {
            new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
            new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
            };

            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };

            var interviewrounds = new List<InterviewRoundViewModel>
            {
            new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
            new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
            };

            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponse1);


            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.Null(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        //4th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Edit_ContactFailedToSaveServiceResponseNull_RedirectToActionSomethingWentWrong()
        {
            //Arrange
            var id = 1;
            var viewModel = new UpdateEmployeeViewModel { EmployeeId = id, FirstName = "C1", LastName = "D1" };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };

            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            var categories = new List<JobRoleViewModel>
            {
            new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
            new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
            };
            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = categories
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);

            var intreview = new List<InterviewRoundViewModel>
            {
            new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
            new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
            };
            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = intreview
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse1);
            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.Null(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Something went wrong please try after some time.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        //5th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void EditPost_ExceptionThrown_RedirectsToHomeIndexWithErrorMessage()
        {
            // Arrange
            var employeeViewModel = new UpdateEmployeeViewModel();
            var exceptionMessage = "Simulated exception message";

            mockHttpClientService.Setup(x => x.PutHttpResponseMessage(It.IsAny<string>(), employeeViewModel, It.IsAny<HttpRequest>()))
                .Throws(new Exception(exceptionMessage));
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            var categories = new List<JobRoleViewModel>
            {
            new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
            new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
            };
            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = categories
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);

            var intreview = new List<InterviewRoundViewModel>
            {
            new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
            new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
            };
            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = intreview
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse1);
            // Act
            var result = target.Edit(employeeViewModel) as RedirectToActionResult;

            // Assert
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }


        //Index
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Index_WithSearch_ReturnsEmployees()
        {
            // Arrange
            var search = "Test";
            var page = 1;
            var pageSize = 5;
            var sortOrder = "asc";
            var totalCount = 10;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var expectedUsers = new List<EmployeeViewModel>
            {
                new EmployeeViewModel { EmployeeId = 1, FirstName = "Test" },
                new EmployeeViewModel { EmployeeId = 2, FirstName = "Test" }
            };

            var countResponse = new ServiceResponse<int> { Success = true, Data = totalCount };
            var usersResponse = new ServiceResponse<IEnumerable<EmployeeViewModel>> { Success = true, Data = expectedUsers };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null , 60)).Returns(countResponse);

            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(usersResponse);
            var controller = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ViewBag.Search = search;
            controller.ViewBag.CurrentPage = page;
            controller.ViewBag.PageSize = pageSize;
            controller.ViewBag.TotalPages = totalPages;
            controller.ViewBag.SortOrder = sortOrder;

            // Act
            var actual = controller.Index(search, page, pageSize, sortOrder) as ViewResult;
           
            // Assert
            Assert.NotNull(actual);
            Assert.Equal(search, controller.ViewBag.Search);
        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Index_SearchNull_ReturnsEmployeesWithoutSearch()
        {
            // Arrange
            var search = (string)null;
            var page = 1;
            var pageSize = 5;
            var sortOrder = "asc";
            var totalCount = 10;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var expectedUsers = new List<EmployeeViewModel>
            {
                new EmployeeViewModel { EmployeeId = 1, FirstName = "Test" },
                new EmployeeViewModel { EmployeeId = 2, FirstName = "Test" }
            };

            var countResponse = new ServiceResponse<int> { Success = true, Data = totalCount };
            var usersResponse = new ServiceResponse<IEnumerable<EmployeeViewModel>> { Success = true, Data = expectedUsers };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);


            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>(
                    It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(usersResponse);


            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            target.ViewBag.Search = search;
            target.ViewBag.CurrentPage = page;
            target.ViewBag.PageSize = pageSize;
            target.ViewBag.TotalPages = totalPages;
            target.ViewBag.SortOrder = sortOrder;

            // Act
            var actual = target.Index(search, page, pageSize, sortOrder) as ViewResult;
            
            // Assert
            Assert.NotNull(actual);
            Assert.Null(target.ViewBag.Search); 
        }
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Index_SearchNull_ReturnsRedirectToAction_WhenResponseIsNUll()
        {
            // Arrange
            var search = (string)null;
            var page = 1;
            var pageSize = 5;
            var sortOrder = "asc";
            var totalCount = 10;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var expectedUsers = new List<EmployeeViewModel>
            {
                new EmployeeViewModel { EmployeeId = 1, FirstName = "Test" },
                new EmployeeViewModel { EmployeeId = 2, FirstName = "Test" }
            };

            var countResponse = new ServiceResponse<int> { Success = true, Data = totalCount };
            
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);


            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<IEnumerable<EmployeeViewModel>>>(
                    It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60)).Returns<IEnumerable<EmployeeViewModel>>(null);


            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            target.ViewBag.Search = search;
            target.ViewBag.CurrentPage = page;
            target.ViewBag.PageSize = pageSize;
            target.ViewBag.TotalPages = totalPages;
            target.ViewBag.SortOrder = sortOrder;

            // Act
            var actual = target.Index(search, page, pageSize, sortOrder) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Null(target.ViewBag.Search);
            Assert.Equal("Index", actual.ActionName);

        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Index_TotalCountZero_ReturnsEmptyView()
        {
            // Arrange
            var search = "Test";
            var page = 1;
            var pageSize = 5;
            var sortOrder = "asc";
            var totalCount = 0;

            var countResponse = new ServiceResponse<int> { Success = true, Data = totalCount };

            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(),HttpMethod.Get,
                    It.IsAny<HttpRequest>(), null, 60)).Returns(countResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            target.ViewBag.Search = search;
            target.ViewBag.CurrentPage = page;
            target.ViewBag.PageSize = pageSize;

            // Act
            var actual = target.Index(search, page, pageSize, sortOrder) as ViewResult;
            var model = actual.Model as List<EmployeeViewModel>;

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(model); 
            Assert.Equal(search, target.ViewBag.Search);
        }


        //4th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Index_PageGreaterThanTotalPages_RedirectsToFirstPage()
        {
            // Arrange
            var search = "Test";
            var page = 2; 
            var pageSize = 5;
            var sortOrder = "asc";
            var totalCount = 1;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var countResponse = new ServiceResponse<int> { Success = true, Data = totalCount };

            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(),
                    HttpMethod.Get,
                    It.IsAny<HttpRequest>(), null, 60))
                .Returns(countResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            target.ViewBag.Search = search;
            target.ViewBag.CurrentPage = page;
            target.ViewBag.PageSize = pageSize;
            target.ViewBag.TotalPages = totalPages;

            // Act
            var result = target.Index(search, page, pageSize, sortOrder) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }


        //5th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Index_ExceptionThrown_RedirectsToHomeIndexWithErrorMessage()
        {
            // Arrange
            var search = "Test";
            var page = 1;
            var pageSize = 5;
            var sortOrder = "asc";

            var exceptionMessage = "Simulated exception message";

            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<int>>(
                    It.IsAny<string>(),
                    HttpMethod.Get,
                    It.IsAny<HttpRequest>(), null, 60))
                .Throws(new Exception(exceptionMessage));

            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            target.ViewBag.Search = search;
            target.ViewBag.CurrentPage = page;
            target.ViewBag.PageSize = pageSize;

            // Act
            var result = target.Index(search, page, pageSize, sortOrder) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Home", result.ControllerName); 
            Assert.Equal("Index", result.ActionName); 
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
        }




        //Delete 
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Delete_SuccessfulResponse_RedirectsToIndexWithSuccessMessage()
        {
            // Arrange
            var employeeId = 1;
            var expectedMessage = "Employee removed successfully.";

            var successResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = expectedMessage
            };

            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Put, It.IsAny<HttpRequest>(), null, 60))
                .Returns(successResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var result = target.Delete(employeeId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); 
            Assert.Equal(expectedMessage, target.TempData["SuccessMessage"]);
        }


        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Delete_UnsuccessfulResponse_RedirectsToIndexWithErrorMessage()
        {
            // Arrange
            var employeeId = 1;
            var expectedErrorMessage = "Failed to remove employee.";

            var errorResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = expectedErrorMessage
            };

            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Put, It.IsAny<HttpRequest>(), null, 60))
                .Returns(errorResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var result = target.Delete(employeeId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); 
            Assert.Equal(expectedErrorMessage, target.TempData["ErrorMessage"]);
        }



        //3rd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void Delete_ExceptionThrown_RedirectsToIndexWithErrorMessage()
        {
            // Arrange
            var employeeId = 1;
            var exceptionMessage = "Simulated exception message";

            mockHttpClientService.Setup(x => x.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Put, It.IsAny<HttpRequest>(), null, 60))
                .Throws(new Exception(exceptionMessage));
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var result = target.Delete(employeeId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); 
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
        }



        //AddInterviewer
        //1st case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddInterviewer_ReturnsViewWithPopulatedViewModel()
        {
            // Arrange
            var jobroles = new List<JobRoleViewModel>
             {
             new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
             new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
             };

            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };

            var interviewrounds = new List<InterviewRoundViewModel>
             {
             new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
             new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
             };

            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponse1);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var result = target.AddInterviewer() as ViewResult;
            var model = result.Model as AddInterviewrViewModel;

            // Assert
            Assert.IsType<AddInterviewrViewModel>(model);
            Assert.Equal(null, result.ViewName);
        }



        //2nd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddInterviewer_InvalidModel_ReturnsView()
        {
            // Arrange
            var jobroles = new List<JobRoleViewModel>
             {
             new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
             new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
             };

            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };

            var interviewrounds = new List<InterviewRoundViewModel>
             {
             new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
             new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
             };

            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponse1);

            var addInterviewrViewModel = new AddInterviewrViewModel();
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            target.ModelState.AddModelError("FirstName", "FirstName is required.");

            // Act
            var result = target.AddInterviewer(addInterviewrViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(result.ViewData.ModelState.IsValid);
            Assert.Equal(addInterviewrViewModel, result.Model);
        }


        //3rd case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddInterviewer_RedirectToActionResult_WhenEmployeeSavedSuccessfully()
        {
            //Arrange
            var jobroles = new List<JobRoleViewModel>
             {
             new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
             new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
             };

            var interviewrounds = new List<InterviewRoundViewModel>
             {
             new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
             new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
             };


            var viewModel = new AddInterviewrViewModel
            {
                FirstName = "Test",
                JobRoleId = 1,
                InterviewRoundId = 1,
                JobRoles = jobroles,
                InterviewRounds = interviewrounds,
                Email = "email@gmail.com"
            };
            var successMessage = "Interviewer created successfully.";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };
            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };
            var expectedResponsee = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponsee);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponse1);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.AddInterviewer(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Interviewer created successfully.", target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }


        //4th case
        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddInterviewer_ReturnsViewResultWithErrorMessage_WhenResponseIsNotSuccess()
        {
            //Arrange
            var jobroles = new List<JobRoleViewModel>
             {
             new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
             new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
             };
            var interviewrounds = new List<InterviewRoundViewModel>
             {
             new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
             new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
             };
            var viewModel = new AddInterviewrViewModel
            {
                FirstName = "Test",
                JobRoleId = 1,
                InterviewRoundId = 1,
                JobRoles = jobroles,
                InterviewRounds = interviewrounds,
                Email = "email@gmail.com"
            };
            var errorMessage = "Something went wrong please try after some time.";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };
            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };
            var expectedResponsee = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponsee);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponse1);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.AddInterviewer(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);

            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddInterviewer_ReturnsViewResultWithErrorMessage_WhenServiceResponseIsNotSuccess()
        {
            //Arrange
            var jobroles = new List<JobRoleViewModel>
             {
             new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
             new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
             };
            var interviewrounds = new List<InterviewRoundViewModel>
             {
             new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
             new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
             };
            var viewModel = new AddInterviewrViewModel
            {
                FirstName = "Test",
                JobRoleId = 1,
                InterviewRoundId = 1,
                JobRoles = jobroles,
                InterviewRounds = interviewrounds,
                Email = "email@gmail.com"
            };
            var errorMessage = "Something went wrong please try after some time.";
           
            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };
            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };
            var expectedResponsee = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponsee);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponse1);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.AddInterviewer(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);

            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        [Trait("Admin", "AdminControllerTests")]
        public void AddInterviewer_ExceptionThrown_RedirectsToHomeIndexWithErrorMessage()
        {
            // Arrange
            var addInterviewrViewModel = new AddInterviewrViewModel(); 
            var jobroles = new List<JobRoleViewModel>
             {
             new JobRoleViewModel { JobRoleId =1, JobRoleName = "C1"},
             new JobRoleViewModel { JobRoleId =2, JobRoleName = "C2"},
             };
            var interviewrounds = new List<InterviewRoundViewModel>
             {
             new InterviewRoundViewModel { InterviewRoundId =1, InterviewRoundName = "C1"},
             new InterviewRoundViewModel { InterviewRoundId =2, InterviewRoundName = "C2"},
             };
            var expectedResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = jobroles,
            };
            var expectedResponse1 = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = interviewrounds,
            };
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
            .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponse1);
            var exceptionMessage = "An unexpected error occurred. Please try again later.";

            mockHttpClientService.Setup(x => x.PostHttpResponseMessage(It.IsAny<string>(), addInterviewrViewModel, It.IsAny<HttpRequest>()))
                .Throws(new Exception(exceptionMessage));
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AdminController(mockHttpClientService.Object, mockConfiguration.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            // Act
            var result = target.AddInterviewer(addInterviewrViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Home", result.ControllerName); 
            Assert.Equal("Index", result.ActionName); 
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
        }




        public void Dispose()
        {
            mockHttpClientService.VerifyAll();
            mockConfiguration.VerifyAll();
            mockTokenHandler.VerifyAll();
            mockHttpContext.VerifyAll();
        }
    }
}
