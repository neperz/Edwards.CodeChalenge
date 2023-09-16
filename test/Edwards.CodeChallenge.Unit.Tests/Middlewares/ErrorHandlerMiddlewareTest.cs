using Edwards.CodeChallenge.API.Middlewares;
 
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Edwards.CodeChallenge.Unit.Tests.Middlewares
{
    public class ErrorHandlerMiddlewareTest
    {
      
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private readonly HttpContext _httpContext;

        public ErrorHandlerMiddlewareTest()
        {
            _httpContext = new DefaultHttpContext().Request.HttpContext;
         
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _webHostEnvironmentMock
                .Setup(x => x.EnvironmentName)
                .Returns("Development");
        }

        private ErrorHandlerMiddleware GetErrorHandlerMiddleware()
        {
            return new ErrorHandlerMiddleware( _webHostEnvironmentMock.Object);
        }

        [Fact]
        public async Task InvokeErrorHandler_ExceptionTest()
        {
            var exceptionHandlerFeature = new ExceptionHandlerFeature()
            {
                Error = new Exception("Mock error exception")
            };

            _httpContext.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);

            var errorHandlerMiddleware = GetErrorHandlerMiddleware();
            await errorHandlerMiddleware.Invoke(_httpContext);

            Assert.NotNull(errorHandlerMiddleware);
        }

        [Fact]
        public async Task InvokeErrorHandler_NotExceptionTest()
        {
            var errorHandlerMiddleware = GetErrorHandlerMiddleware();
            await errorHandlerMiddleware.Invoke(_httpContext);

            Assert.NotNull(errorHandlerMiddleware);
        }
    }
}
