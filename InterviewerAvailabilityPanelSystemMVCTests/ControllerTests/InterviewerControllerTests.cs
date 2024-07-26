using InterviewPanelAvailabilitySystemMVC.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewPanelAvailabilitySystemMVCTests.ControllerTests
{
    public class InterviewerControllerTests
    {
        [Fact]
        [Trait("Interviewer", "InterviewerControllerTests")]
        public void Index_ReturnsView()
        {
            var target = new InterviewerController();
           
            var actual = target.Index() as ViewResult;

            Assert.IsType<ViewResult>(actual);
        }

        

        [Fact]
        public void Index_CatchesException_RedirectsToHomeWithError()
        {
            // Arrange
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };

            // Create a mocked instance of InterviewerController with mock dependencies
            var mockController = new Mock<InterviewerController>
            {
                CallBase = true // Ensures that methods not explicitly set up will still call the base implementation
            };
            mockController.Object.TempData = tempData;
            mockController.Object.ControllerContext = controllerContext;

            mockController.Setup(c => c.View()).Throws(new Exception("Simulated exception"));

            // Act
            var result = mockController.Object.Index() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName); // Ensure it redirects to Home controller
            Assert.Equal("An unexpected error occurred. Please try again later.", mockController.Object.TempData["ErrorMessage"]);
        }

    }
}
