using Edwards.CodeChallenge.API.Services;
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

namespace Edwards.CodeChallenge.Unit.Tests.Services
{
    public class EdwardsUserServiceTest : ConfigBase
    {
        private readonly Mock<IEdwardsUserRepository> _edwardsUserRepositoryMock;

        private readonly Mock<IDomainNotification> _domainNotificationMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ConcurrentDictionary<int, EdwardsUserViewModel>> _cache;

        public EdwardsUserServiceTest()
        {
            _edwardsUserRepositoryMock = new Mock<IEdwardsUserRepository>();

            _domainNotificationMock = new Mock<IDomainNotification>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cache = new Mock<ConcurrentDictionary<int, EdwardsUserViewModel>>();
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
        public async Task GetAll_ReturnUsersViewModelTestAsync()
        {

            _edwardsUserRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(EdwardsUserMock.EdwardsUserModelFaker.Generate(3));



            var edwardsUserService = GetEdwardsUserService();

            var customeMethod = await edwardsUserService.GetAllAsync();

            var edwardsUserResult = Assert.IsAssignableFrom<IEnumerable<EdwardsUserViewModel>>(customeMethod);

            Assert.NotNull(edwardsUserResult);
            Assert.NotEmpty(edwardsUserResult);
        }

        [Fact]
        public async Task GetById_ReturnEdwardsUserViewModelTestAsync()
        {
            var edwardsUserId = EdwardsUserMock.EdwardsUserIdViewModelFaker.Generate();

            _edwardsUserRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(EdwardsUserMock.EdwardsUserModelFaker.Generate());

            var edwardsUserService = GetEdwardsUserService();

            var customeMethod = await edwardsUserService.GetByIdAsync(edwardsUserId);

            var edwardsUserResult = Assert.IsAssignableFrom<EdwardsUserViewModel>(customeMethod);

            Assert.NotNull(edwardsUserResult);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnEdwardsUserViewModelTestAsync()
        {
            var edwardsUserId = EdwardsUserMock.EdwardsUserIdViewModelFaker.Generate();

            _edwardsUserRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(EdwardsUserMock.EdwardsUserModelFaker.Generate());

            var edwardsUserService = GetEdwardsUserService();

            var customeMethod = await edwardsUserService.GetByIdAsync(edwardsUserId);

            var edwardsUserResult = Assert.IsAssignableFrom<EdwardsUserViewModel>(customeMethod);

            Assert.NotNull(edwardsUserResult);
        }

        [Fact]
        public async Task GetUserByNameAsync_ReturnEdwardsUserViewModelTestAsync()
        {
            var edwardsUserName = EdwardsUserMock.EdwardsUserNameViewModelFaker.Generate();

            _edwardsUserRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(EdwardsUserMock.EdwardsUserModelFaker.Generate());

            var edwardsUserService = GetEdwardsUserService();

            var customeMethod = await edwardsUserService.GetByNameAsync(edwardsUserName);

            var edwardsUserResult = Assert.IsAssignableFrom<EdwardsUserViewModel>(customeMethod);

            Assert.NotNull(edwardsUserResult);
        }
        [Fact]
        public async Task GetUserByEmailAsync_ReturnEdwardsUserViewModelTestAsync()
        {
            var edwardsUserEmail = EdwardsUserMock.EdwardsUserEmailViewModelFaker.Generate();

            _edwardsUserRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(EdwardsUserMock.EdwardsUserModelFaker.Generate());

            var edwardsUserService = GetEdwardsUserService();

            var customeMethod = await edwardsUserService.GetByEmailAsync(edwardsUserEmail);

            var edwardsUserResult = Assert.IsAssignableFrom<EdwardsUserViewModel>(customeMethod);

            Assert.NotNull(edwardsUserResult);
        }

        [Fact]
        public async Task Add_ReturnEdwardsUserViewModelTestAsync()
        {
            var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();

            _edwardsUserRepositoryMock.Setup(x => x.GetByNameAsync(edwardsUser.FirstName))
                .ReturnsAsync(EdwardsUserMock.EdwardsUserModelFaker.Generate());

            var edwardsUserService = GetEdwardsUserService();

            await edwardsUserService.AddAsync(edwardsUser);

            Assert.NotNull(edwardsUser);
        }

        [Fact]
        public async Task Update_SucessTestAsync()
        {
            var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();

            var edwardsUserService = GetEdwardsUserService();

            await edwardsUserService.UpdateAsync(edwardsUser);

            Assert.NotNull(edwardsUser);
        }

        [Fact]
        public async Task Remove_SucessTestAsync()
        {
            var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();

            var edwardsUserService = GetEdwardsUserService();

            await edwardsUserService.RemoveAsync(edwardsUser);

            Assert.NotNull(edwardsUser);
        }
    }
}
