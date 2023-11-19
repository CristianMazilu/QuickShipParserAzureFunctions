using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickShipParser;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;


namespace QuickShipParser.Facts
{
    [TestClass()]
    public class QuickShipParseFacts
    {
        [TestMethod]
        public async Task Run_NoParameters_ReturnsBadRequest()
        {
            // Arrange
            var context = new DefaultHttpContext(); // Create a new HttpContext
            var request = context.Request; // Get the HttpRequest from the HttpContext

            // Set up the empty Query collection
            context.Request.Query = new QueryCollection(new Dictionary<string, StringValues>());

            // Mock ILogger
            var loggerMock = new Mock<ILogger>();

            // Act
            var result = await QuickShipParse.Run(request, loggerMock.Object);

            // Assert
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
        }


        [TestMethod]
        public async Task Run_ModelParameter_ReturnsOkResult()
        {
            var queryStringValue = new StringValues("8705THA015");
            var queryDict = new Dictionary<string, StringValues> { { "model", queryStringValue } };
            var queryCollection = new QueryCollection(queryDict);

            var context = new DefaultHttpContext(); // Create a new HttpContext
            context.Request.Query = queryCollection; // Set the Query collection

            var loggerMock = new Mock<ILogger>();

            // Act
            var result = await QuickShipParse.Run(context.Request, loggerMock.Object);

            // Assert
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        }
    }
}