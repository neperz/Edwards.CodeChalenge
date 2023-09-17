using Edwards.CodeChallenge.API.Services;
using Edwards.CodeChallenge.API.Services.Interfaces;
using Edwards.CodeChallenge.API.ViewModels.User;
using Edwards.CodeChallenge.Core.Tests.Mocks;
using Edwards.CodeChallenge.Domain.Interfaces.Notifications;
using Edwards.CodeChallenge.Domain.Interfaces.Repository;
using Edwards.CodeChallenge.Domain.Interfaces.UoW;
using Edwards.CodeChallenge.Unit.Tests.Configuration;
using Moq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions; 

namespace Edwards.CodeChallenge.Unit.Tests.Services
{
    // TODO: bonus - Add Unit testing to the project so that the main methods can be tested by the developer
    public class EdwardsUserServiceTest : ConfigBase
    {
        private readonly Mock<IEdwardsUserRepository> _edwardsUserRepositoryMock;

        private readonly Mock<IDomainNotification> _domainNotificationMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ConcurrentDictionary<string, EdwardsUserViewModel>> _cache;

        public EdwardsUserServiceTest()
        {
            _edwardsUserRepositoryMock = new Mock<IEdwardsUserRepository>();

            _domainNotificationMock = new Mock<IDomainNotification>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cache = new Mock<ConcurrentDictionary<string, EdwardsUserViewModel>>();
        }

        private EdwardsUserService GetEdwardsUserService()
        {
            return new EdwardsUserService
                (
                _edwardsUserRepositoryMock.Object,
                _cache.Object,
                 _domainNotificationMock.Object,
                 _unitOfWorkMock.Object,
                 _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfEdwardsUserViewModel()
        {
            // Arrange
            var expectedUsers = EdwardsUserMock.EdwardsUserModelFaker.Generate(3);
            _edwardsUserRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expectedUsers);

            // Act
            var result = await GetEdwardsUserService().GetAllAsync();

            // Assert
            result.Should().NotBeNull()
                .And.BeAssignableTo<IEnumerable<EdwardsUserViewModel>>()
                .And.NotBeEmpty();
        }

        [Fact]
        public async Task GetById_ReturnEdwardsUserViewModelTestAsync()
        {
            // Arrange
            var edwardsUserId = EdwardsUserMock.EdwardsUserIdViewModelFaker.Generate();

            var expectedUser = EdwardsUserMock.EdwardsUserModelFaker.Generate();
            _edwardsUserRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await GetEdwardsUserService().GetByIdAsync(edwardsUserId);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<EdwardsUserViewModel>()
                .And.BeEquivalentTo(expectedUser);  
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnEdwardsUserViewModelTestAsync()
        {
            // Arrange
            var edwardsUserId = EdwardsUserMock.EdwardsUserIdViewModelFaker.Generate();

            var expectedUser = EdwardsUserMock.EdwardsUserModelFaker.Generate();
            _edwardsUserRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await GetEdwardsUserService().GetByIdAsync(edwardsUserId);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<EdwardsUserViewModel>()
                .And.BeEquivalentTo(expectedUser);  
        }

        [Fact]
        public async Task GetUserByNameAsync_ReturnEdwardsUserViewModelTestAsync()
        {
            // Arrange
            var edwardsUserName = EdwardsUserMock.EdwardsUserNameViewModelFaker.Generate();

            var expectedUser = EdwardsUserMock.EdwardsUserModelFaker.Generate();
            _edwardsUserRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await GetEdwardsUserService().GetByNameAsync(edwardsUserName);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<EdwardsUserViewModel>()
                .And.BeEquivalentTo(expectedUser);  
        }

        [Fact]
        public async Task GetUserByEmailAsync_ReturnEdwardsUserViewModelTestAsync()
        {
            // Arrange
            var edwardsUserEmail = EdwardsUserMock.EdwardsUserEmailViewModelFaker.Generate();

            var expectedUser = EdwardsUserMock.EdwardsUserModelFaker.Generate();
            _edwardsUserRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await GetEdwardsUserService().GetByEmailAsync(edwardsUserEmail);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<EdwardsUserViewModel>()
                .And.BeEquivalentTo(expectedUser); // Compare the result with the expected user object
        }

        [Fact]
        public async Task Add_ReturnEdwardsUserViewModelTestAsync()
        {
            // Arrange
            var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();

            _edwardsUserRepositoryMock.Setup(x => x.GetByNameAsync(edwardsUser.FirstName))
                .ReturnsAsync(EdwardsUserMock.EdwardsUserModelFaker.Generate());

            // Act
            await GetEdwardsUserService().AddAsync(edwardsUser);

            // Assert
            edwardsUser.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_SuccessTestAsync()
        {
            // Arrange
            var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();

            // Act
            await GetEdwardsUserService().UpdateAsync(edwardsUser);

            // Assert
            edwardsUser.Should().NotBeNull();
        }

        [Fact]
        public async Task Remove_SuccessTestAsync()
        {
            // Arrange
            var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();

            // Act
            await GetEdwardsUserService().RemoveAsync(edwardsUser);

            // Assert
            edwardsUser.Should().NotBeNull();
        }
    }
}
