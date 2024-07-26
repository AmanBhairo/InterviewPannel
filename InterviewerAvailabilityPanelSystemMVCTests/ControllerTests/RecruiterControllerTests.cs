using InterviewPanelAvailabilitySystemMVC.Controllers;
using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace InterviewPanelAvailabilitySystemMVCTests.ControllerTests
{
    public class RecruiterControllerTests : IDisposable
    {
        private readonly Mock<IHttpClientService> mockHttpClientService;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<IJwtTokenHandler> mockTokenHandler;
        private readonly Mock<HttpContext> mockHttpContext;

        public RecruiterControllerTests()
        {
            mockHttpClientService = new Mock<IHttpClientService>();
            mockConfiguration = new Mock<IConfiguration>();
            mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpContext = new Mock<HttpContext>();
        }


        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNotNull_RoundIsNotNull_JobRoleIdIsNotNull()
        {
            // Arrange
            int jobRoleId = 1;
            int interviewRoundId = 1;
            int page = 1;
            int pageSize = 2;
            string? search = "f";
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService
              .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                  It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
              .Returns(new ServiceResponse<int> { Success = true, Data = 0 });
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);

            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(jobRoleId, interviewRoundId, page, pageSize, sort, search) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
             .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
                 It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNotNull_RoundIsNotNull_JobRoleIdIsNull()
        {
            // Arrange

            int interviewRoundId = 1;
            int page = 1;
            int pageSize = 2;
            string? search = "f";
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 0 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(null, interviewRoundId, page, pageSize, sort, search) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 0 });
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNotNull_RoundIsNull_JobRoleIdIsNotNull()
        {
            // Arrange
            int jobRoleId = 1;
            int page = 1;
            int pageSize = 2;
            string? search = "f";
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 0 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(jobRoleId, null, page, pageSize, sort, search) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
 .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNotNull_RoundIsNull_JobRoleIdIsNull()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string? search = "f";
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 0 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(null, null, page, pageSize, sort, search) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
 .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNull_RoundIsNotNull_JobRoleIdIsNotNull()
        {
            // Arrange
            int jobRoleId = 1;
            int interviewRoundId = 1;
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 1 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(jobRoleId, interviewRoundId, page, pageSize, sort, null) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
 .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNull_RoundIsNotNull_JobRoleIdIsNull()

        {
            // Arrange
            int interviewRoundId = 1;
            int page = 3;
            int pageSize = 2;
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 1 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(null, interviewRoundId, page, pageSize, sort, null) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
 .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNull_RoundIsNull_JobRoleIdIsNotNull()
        {
            // Arrange
            int jobRoleId = 1;
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
 .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
 .Returns(new ServiceResponse<int> { Success = true, Data = 1 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(jobRoleId, null, page, pageSize, sort, null) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
 .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenSearchIsNull_RoundIsNull_JobRoleIdIsNull()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 0 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(null, null, page, pageSize, sort, null) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
 .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
        }


        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void Index_ReturnsEmptyList_WhenLetterIsNull()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var expectedCountries = new List<JobRoleViewModel>
                {
                    new JobRoleViewModel{},

                };
            var expectedStates = new List<InterviewRoundViewModel>
                {
                    new InterviewRoundViewModel{},

                };
            var stateResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedStates
            };

            var countryResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedCountries
            };
            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = false,
                Data = expectedCategories
            };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(countryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(stateResponse);
            mockHttpClientService
  .Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(
      It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
  .Returns(new ServiceResponse<int> { Success = true, Data = 1 });
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(null, null, page, pageSize, sort, null) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService
 .Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(
     It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void UpdateInterviewSlot_ReturnsView_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var viewModel = new InterviewSlotsViewModel { SlotId = 1, SlotDate = DateTime.Now };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<InterviewSlotsViewModel>
            {
                Data = viewModel,
                Success = true
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdateInterviewSlot(id) as ViewResult;

            //Assert
            var model = actual.Model as InterviewSlotsViewModel;
            Assert.NotNull(model);
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Index_ReturnsException()
        {
            // Arrange
            int jobRoleId = 1;
            int interviewRoundId = 1;
            int page = 1;
            int pageSize = 2;
            string? search = "f";
            string sort = "asc";

            var expectedCategories = new List<InterviewSlotsViewModel> { };

            var expectedResponse = new ServiceResponse<IEnumerable<InterviewSlotsViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService
                .Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewSlotsViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Throws(new Exception());

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var errorMessage = "An unexpected error occurred. Please try again later.";
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,

                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.Index(jobRoleId, interviewRoundId, page, pageSize, sort, search) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);

            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void UpdateInterviewSlot_ReturnsRedirectToAction_WhenStatusCodeIsSuccess()
        {
            var id = 1;
            var viewModel = new InterviewSlotsViewModel { SlotId = 1, SlotDate = DateTime.Now };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var errorMessage = "Timeslot selected successfully";
            var expectedServiceResponse = new ServiceResponse<InterviewSlotsViewModel>
            {
                Data = viewModel,
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdateInterviewSlot(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void UpdateInterviewSlot_ReturnsRedirectToAction_WhenStatusCodeIsNotSuccess_ErrorResponseIsNotNull()
        {
            var id = 1;
            var viewModel = new InterviewSlotsViewModel { SlotId = 1, SlotDate = DateTime.Now };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var expectedServiceResponse = new ServiceResponse<InterviewSlotsViewModel>
            {
                Data = viewModel,
                Success = false
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdateInterviewSlot(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(null, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void UpdateInterviewSlot_ReturnsRedirectToAction_WhenStatusCodeIsNotSuccess_ErrorResponseIsNull()
        {
            var id = 1;
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var errorMessage = "Something went wrong please try after some time.";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedReponse);

            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdateInterviewSlot(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateInterviewSlot_ReturnsExceptions()
        {
            var id = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var errorMessage = "An unexpected error occurred. Please try again later.";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Throws(new Exception());

            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdateInterviewSlot(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<InterviewSlotsViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void UpdateInterviewSlot_SlotUpdatedSuccessfully_RedirectToAction()
        {
            //Arrange
            var id = 1;
            var viewModel = new InterviewSlotsViewModel { SlotId = id, SlotDate = DateTime.Now };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Slot updated successfully";
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
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateInterviewSlot(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void UpdateInterviewSlot_RedirectToAction_WhenServiceResponseNotNull()
        {
            //Arrange
            var id = 1;
            var viewModel = new InterviewSlotsViewModel { SlotId = id, SlotDate = DateTime.Now };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
            };
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateInterviewSlot(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(null, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        [Trait("Recruiter", "RecruiterControllerTests")]
        public void UpdateInterviewSlot_RedirectToAction_WhenServiceResponseNull()
        {
            //Arrange
            var id = 1;
            var viewModel = new InterviewSlotsViewModel { SlotId = id, SlotDate = DateTime.Now };

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Something went wrong. Please try after sometime.";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedReponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateInterviewSlot(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateInterviewSlot_ReturnsException()
        {
            //Arrange
            var id = 1;
            var viewModel = new InterviewSlotsViewModel { SlotId = id, SlotDate = DateTime.Now };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "An unexpected error occurred. Please try again later.";
            var expectedReponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Throws(new Exception());
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);
            var target = new RecruiterController(mockHttpClientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            //Act
            var actual = target.UpdateInterviewSlot(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
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
