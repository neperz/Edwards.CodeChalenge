using Edwards.CodeChallenge.Domain.Models;
using Edwards.CodeChallenge.Infra;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace Edwards.CodeChallenge.Unit.Tests.Services
{
    // TODO: bonus - Add Unit testing to the project so that the main methods can be tested by the developer
    public class FileServiceTests
    {
        [Fact]
        public void DumpDataToDisk_Success()
        {
            // Arrange
            var config = new FileConfig
            {
                FilePath = "test-path"                
            };

            var mockOptions = new Mock<IOptions<FileConfig>>();
            mockOptions.Setup(x => x.Value).Returns(config);

            var fileService = new FileService(mockOptions.Object);

            // Act
            var result = fileService.DumpDataToDisk("sample data");  

            // Assert
            Assert.True(result.Success);
            Assert.Null(result.ErrorMessage);
        }

       
        [Fact]
        public void DumpDataToDisk_Failure()
        {
            // Arrange
            var errorMessage = "Error writing data to file.";
            var mockOptions = new Mock<IOptions<FileConfig>>();
            mockOptions.Setup(x => x.Value).Returns(new FileConfig { FilePath = "" });

            var fileService = new FileService(mockOptions.Object);

            // Act
            var result = fileService.DumpDataToDisk("sample data"); 

            // Assert
            Assert.False(result.Success);
            Assert.Contains(errorMessage, result.ErrorMessage);
        }

    }

}
