using AutoFixture;
using InterviewPanelAvailabilitySystemMVC.Controllers;
using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemMVCTests.ControllerTests
{
    public class ReportControllerTests
    {
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedReport_ReturnsReport_WhenFetchedSuccessfully()
        {
            //Arrange
            int? jobRoleId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedJobRoles = new List<JobRoleViewModel>
            {
                new JobRoleViewModel{ JobRoleId=1,JobRoleName="Role1"},
                new JobRoleViewModel{ JobRoleId=2,JobRoleName="Role2"},
                new JobRoleViewModel{ JobRoleId=3,JobRoleName="Role3"},
            };

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = fixture.CreateMany<DetailedReportViewModel>();

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>> 
            { 
                Success=true,
                Data=expectedJobRoles,
                Message =""
            };

            var expectedCountResponse = new ServiceResponse<int> 
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = true,
                Data = expectedReport,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedJobRoleResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedReport(jobRoleId,booked,page,pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedReport_ReturnsReport_PageIsGreaterThanTotalPages()
        {
            //Arrange
            int? jobRoleId = 1;
            bool booked = false;
            int page = 3;
            int pageSize = 4;

            var expectedJobRoles = new List<JobRoleViewModel>
            {
                new JobRoleViewModel{ JobRoleId=1,JobRoleName="Role1"},
                new JobRoleViewModel{ JobRoleId=2,JobRoleName="Role2"},
                new JobRoleViewModel{ JobRoleId=3,JobRoleName="Role3"},
            };

            int expectedCount = 5;

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedJobRoles,
                Message = ""
            };

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedJobRoleResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedReport(jobRoleId, booked, page, pageSize) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("DetailedReport", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
           
        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedReport_ReturnsEmptyJobRole()
        {
            //Arrange
            int? jobRoleId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedJobRoles = new List<JobRoleViewModel>();

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = new List<DetailedReportViewModel>();

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = false,
                Data = expectedJobRoles,
                Message = "something went wrong please try after some time."
            };

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = false,
                Data = expectedReport,
                Message = "Something went wrong please try after some time."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedJobRoleResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedReport(jobRoleId, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedReport_ReturnsEmptyReportlist_WhenNoRecordFound()
        {
            //Arrange
            int? jobRoleId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedJobRoles = new List<JobRoleViewModel>();

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = new List<DetailedReportViewModel>();

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedJobRoles,
                Message = ""
            };

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = false,
                Data = expectedReport,
                Message = "No record found."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedJobRoleResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedReport(jobRoleId, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetaildReport_RedirectToHomeIndex_WhenErrorOccurs()
        {
            //Arrange
            int? jobRoleId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedJobRoles = new List<JobRoleViewModel>
            {
                new JobRoleViewModel{ JobRoleId=1,JobRoleName="Role1"},
                new JobRoleViewModel{ JobRoleId=2,JobRoleName="Role2"},
                new JobRoleViewModel{ JobRoleId=3,JobRoleName="Role3"},
            };

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedJobRoles,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var exception = new Exception("Object reference not set to an instance of an object");
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60)).Throws(exception);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedReport(jobRoleId, booked, page, pageSize) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            
        }

        //------------------------------
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedInterviewRoundReport_ReturnsReport_WhenFetchedSuccessfully()
        {
            //Arrange
            int? interviewRoundId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedInterviewRound = new List<InterviewRoundViewModel>
            {
                new InterviewRoundViewModel{ InterviewRoundId=1,InterviewRoundName="Role1"},
                new InterviewRoundViewModel{ InterviewRoundId=2,InterviewRoundName="Role2"},
                new InterviewRoundViewModel{ InterviewRoundId=3,InterviewRoundName="Role3"},
            };

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = fixture.CreateMany<DetailedReportViewModel>();

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = true,
                Data = expectedReport,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedInterviewRoundResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedInterviewRoundReport(interviewRoundId, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedInterviewRoundReport_ReturnsReport_PageIsGreaterThanTotalPages()
        {
            //Arrange
            int? interviewRoundId = 1;
            bool booked = false;
            int page = 3;
            int pageSize = 4;

            var expectedInterviewRound = new List<InterviewRoundViewModel>
            {
                new InterviewRoundViewModel{ InterviewRoundId=1,InterviewRoundName="Role1"},
                new InterviewRoundViewModel{ InterviewRoundId=2,InterviewRoundName="Role2"},
                new InterviewRoundViewModel{ InterviewRoundId=3,InterviewRoundName="Role3"},
            };

            int expectedCount = 5;

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedInterviewRoundResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedInterviewRoundReport(interviewRoundId, booked, page, pageSize) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("DetailedInterviewRoundReport", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedInterviewRoundReport_ReturnsEmptyInterviewRound()
        {
            //Arrange
            int? interviewRoundId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedInterviewRound = new List<InterviewRoundViewModel>();

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = new List<DetailedReportViewModel>();

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = false,
                Data = expectedInterviewRound,
                Message = "something went wrong please try after some time."
            };

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = false,
                Data = expectedReport,
                Message = "Something went wrong please try after some time."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedInterviewRoundResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedInterviewRoundReport(interviewRoundId, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedInterviewRoundReport_ReturnsEmptyReportlist_WhenNoRecordFound()
        {
            //Arrange
            int? interviewRoundId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedInterviewRound = new List<InterviewRoundViewModel>
            {
                new InterviewRoundViewModel{ InterviewRoundId=1,InterviewRoundName="Role1"},
                new InterviewRoundViewModel{ InterviewRoundId=2,InterviewRoundName="Role2"},
                new InterviewRoundViewModel{ InterviewRoundId=3,InterviewRoundName="Role3"},
            };

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = fixture.CreateMany<DetailedReportViewModel>();

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = false,
                Data = expectedReport,
                Message = "No record found."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedInterviewRoundResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedInterviewRoundReport(interviewRoundId, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedInterviewRoundReport_RedirectToHomeIndex_WhenErrorOccurs()
        {
            //Arrange
            int? interviewRoundId = 1;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var expectedInterviewRound = new List<InterviewRoundViewModel>
            {
                new InterviewRoundViewModel{ InterviewRoundId=1,InterviewRoundName="Role1"},
                new InterviewRoundViewModel{ InterviewRoundId=2,InterviewRoundName="Role2"},
                new InterviewRoundViewModel{ InterviewRoundId=3,InterviewRoundName="Role3"},
            };

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = fixture.CreateMany<DetailedReportViewModel>();

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var exception = new Exception("Object reference not set to an instance of an object");
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60)).Throws(exception);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedInterviewRoundReport(interviewRoundId, booked, page, pageSize) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        //------------------------------
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedDateRangeBasedReport_ReturnsReport_WhenFetchedSuccessfully()
        {
            //Arrange
            string? startDate = "01-01-2001";
            string? endDate = "01-01-2002";
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = fixture.CreateMany<DetailedReportViewModel>();

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = true,
                Data = expectedReport,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();


            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedDateRangeBasedReport(startDate,endDate, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedDateRangeBasedReport_ReturnsReport_PageIsGreaterThanTotalPages()
        {
            //Arrange
            string? startDate = "01-01-2001";
            string? endDate = "01-01-2002";
            bool booked = false;
            int page = 3;
            int pageSize = 4;

            int expectedCount = 5;

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedDateRangeBasedReport(startDate,endDate, booked, page, pageSize) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("DetailedDateRangeBasedReport", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedDateRangeBasedReport_ReturnsEmptyReportlist_WhenNoRecordFound()
        {
            //Arrange
            string? startDate = "01-01-2001";
            string? endDate = "01-01-2002";
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = fixture.CreateMany<DetailedReportViewModel>();

            var expectedCountResponse = new ServiceResponse<int>
            {
                Success = true,
                Data = expectedCount
            };

            var expectedReportResponse = new ServiceResponse<IEnumerable<DetailedReportViewModel>>
            {
                Success = false,
                Data = expectedReport,
                Message = "No record found."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedCountResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedDateRangeBasedReport(startDate,endDate, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<DetailedReportViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedDateRangeBasedReport_ReturnsEmptyReportlist_WhenStartDateAndEndDateIsNUll()
        {
            //Arrange
            string? startDate = null;
            string? endDate = null;
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedDateRangeBasedReport(startDate, endDate, booked, page, pageSize) as ViewResult;

            // Assert
            Assert.NotNull(actual);
        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DetailedDateRangeBasedReport_RedirectToHomeIndex_WhenErrorOccurs()
        {
            //Arrange
            string? startDate = "01-01-2001";
            string? endDate = "01-01-2002";
            bool booked = false;
            int page = 1;
            int pageSize = 4;

            int expectedCount = 5;

            var fixture = new Fixture();
            var expectedReport = fixture.CreateMany<DetailedReportViewModel>();


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var exception = new Exception("Object reference not set to an instance of an object");
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60)).Throws(exception);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DetailedDateRangeBasedReport(startDate,endDate, booked, page, pageSize) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        //------------------------------
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void JobRoleBasedCountReportReport_ReturnsReport_WhenFetchedSuccessfully()
        {
            //Arrange
            int? jobRoleId = 1;

            var expectedJobRoles = new List<JobRoleViewModel>
            {
                new JobRoleViewModel{ JobRoleId=1,JobRoleName="Role1"},
                new JobRoleViewModel{ JobRoleId=2,JobRoleName="Role2"},
                new JobRoleViewModel{ JobRoleId=3,JobRoleName="Role3"},
            };

            var fixture = new Fixture();
            var expectedReport = fixture.Create<SlotCountReportViewModel>();

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedJobRoles,
                Message = ""
            };


            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = true,
                Data = expectedReport,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedJobRoleResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.JobRoleBasedCountReportReport(jobRoleId) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void JobRoleBasedCountReportReport_ReturnsEmptyJobRole()
        {
            //Arrange
            int? jobRoleId = 1;
            var expectedJobRoles = new List<JobRoleViewModel>();

            var fixture = new Fixture();
            var expectedReport = new SlotCountReportViewModel();

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = false,
                Data = expectedJobRoles,
                Message = "something went wrong please try after some time."
            };

            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = false,
                Data = expectedReport,
                Message = "Something went wrong please try after some time."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedJobRoleResponse);


            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.JobRoleBasedCountReportReport(jobRoleId) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void JobRoleBasedCountReportReport_ReturnsEmptyReportlist_WhenNoRecordFound()
        {
            //Arrange
            int? jobRoleId = 1;
            var expectedJobRoles = new List<JobRoleViewModel>();

            var fixture = new Fixture();
            var expectedReport = new SlotCountReportViewModel();

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedJobRoles,
                Message = ""
            };

            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = false,
                Data = expectedReport,
                Message = "No record found."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedJobRoleResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.JobRoleBasedCountReportReport(jobRoleId) as ViewResult;


            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void JobRoleBasedCountReportReport_RedirectToHomeIndex_WhenErrorOccurs()
        {
            //Arrange
            int? jobRoleId = 1;
            
            var expectedJobRoles = new List<JobRoleViewModel>
            {
                new JobRoleViewModel{ JobRoleId=1,JobRoleName="Role1"},
                new JobRoleViewModel{ JobRoleId=2,JobRoleName="Role2"},
                new JobRoleViewModel{ JobRoleId=3,JobRoleName="Role3"},
            };

            var expectedJobRoleResponse = new ServiceResponse<IEnumerable<JobRoleViewModel>>
            {
                Success = true,
                Data = expectedJobRoles,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var exception = new Exception("Object reference not set to an instance of an object");
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60)).Throws(exception);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.JobRoleBasedCountReportReport(jobRoleId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<JobRoleViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        //------------------------------
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void InterviewRoundBasedCountReportReport_ReturnsReport_WhenFetchedSuccessfully()
        {
            //Arrange
            int? interviewRoundId = 1;

            var expectedInterviewRound = new List<InterviewRoundViewModel>
            {
                new InterviewRoundViewModel{ InterviewRoundId=1,InterviewRoundName="Role1"},
                new InterviewRoundViewModel{ InterviewRoundId=2,InterviewRoundName="Role2"},
                new InterviewRoundViewModel{ InterviewRoundId=3,InterviewRoundName="Role3"},
            };

            var fixture = new Fixture();
            var expectedReport = fixture.Create<SlotCountReportViewModel>();

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };


            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = true,
                Data = expectedReport,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedInterviewRoundResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.InterviewRoundBasedCountReportReport(interviewRoundId) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void InterviewRoundBasedCountReportReport_ReturnsEmptyJobRole()
        {
            //Arrange
            int? interviewRoundId = 1;
            var expectedInterviewRound = new List<InterviewRoundViewModel>();

            var fixture = new Fixture();
            var expectedReport = new SlotCountReportViewModel();

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };

            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = false,
                Data = expectedReport,
                Message = "Something went wrong please try after some time."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedInterviewRoundResponse);


            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.InterviewRoundBasedCountReportReport(interviewRoundId) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void InterviewRoundBasedCountReportReport_ReturnsEmptyReportlist_WhenNoRecordFound()
        {
            //Arrange
            int? interviewRoundId = 1;
            var expectedInterviewRound = new List<InterviewRoundViewModel>();

            var fixture = new Fixture();
            var expectedReport = new SlotCountReportViewModel();

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };

            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = false,
                Data = expectedReport,
                Message = "No record found."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedInterviewRoundResponse);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.InterviewRoundBasedCountReportReport(interviewRoundId) as ViewResult;


            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void InterviewRoundBasedCountReportReport_RedirectToHomeIndex_WhenErrorOccurs()
        {
            //Arrange
            int? interviewRoundId = 1;

            var expectedInterviewRound = new List<InterviewRoundViewModel>
            {
                new InterviewRoundViewModel{ InterviewRoundId=1,InterviewRoundName="Role1"},
                new InterviewRoundViewModel{ InterviewRoundId=2,InterviewRoundName="Role2"},
                new InterviewRoundViewModel{ InterviewRoundId=3,InterviewRoundName="Role3"},
            };

            var expectedInterviewRoundResponse = new ServiceResponse<IEnumerable<InterviewRoundViewModel>>
            {
                Success = true,
                Data = expectedInterviewRound,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var exception = new Exception("Object reference not set to an instance of an object");
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60)).Throws(exception);


            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.InterviewRoundBasedCountReportReport(interviewRoundId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(2));
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<InterviewRoundViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        //------------------------------
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DateRangeBasedCountReportReport_ReturnsReport_WhenFetchedSuccessfully()
        {
            //Arrange
            string? startDate = "01-01-2001";
            string? endDate = "01-01-2002";

            var fixture = new Fixture();
            var expectedReport = fixture.Create<SlotCountReportViewModel>();

            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = true,
                Data = expectedReport,
                Message = ""
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DateRangeBasedCountReportReport(startDate,endDate) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DateRangeBasedCountReportReport_ReturnsEmptyReportlist_WhenNoRecordFound()
        {
            //Arrange
            string? startDate = "01-01-2001";
            string? endDate = "01-01-2002";

            var fixture = new Fixture();
            var expectedReport = new SlotCountReportViewModel();

            var expectedReportResponse = new ServiceResponse<SlotCountReportViewModel>
            {
                Success = false,
                Data = expectedReport,
                Message = "No record found."
            };


            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            mockHttpCLientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(expectedReportResponse);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DateRangeBasedCountReportReport(startDate,endDate) as ViewResult;


            // Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpCLientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<SlotCountReportViewModel>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        [Trait("Reports", "ReportControllerTests")]
        public void DateRangeBasedCountReportReport_RedirectToHomeIndex_WhenErrorOccurs()
        {
            //Arrange
            string? startDate = "01-01-2001";
            string? endDate = "01-01-2002";

            var mockHttpCLientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakePoint");
            var mockHttpContext = new Mock<HttpContext>();
            var exception = new Exception("Object reference not set to an instance of an object");
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);

            var target = new ReportController(mockHttpCLientService.Object, mockConfiguration.Object)
            {
                TempData = tempData,
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act
            var actual = target.DateRangeBasedCountReportReport(startDate,endDate) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Equal("An unexpected error occurred. Please try again later.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            
        }

    }
}
