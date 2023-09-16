using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Edwards.CodeChalenge.Core.Tests.Mocks;
using Edwards.CodeChalenge.Core.Tests.Mocks.Factory;
using Edwards.CodeChalenge.Infra.Context;
using Edwards.CodeChalenge.Infra.Repository;
using Edwards.CodeChalenge.Infra.UoW;
using Xunit;

namespace Edwards.CodeChalenge.Integration.Tests.Repository
{
    public class CustomerRepositoryTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly DbContextOptions<EntityContext> _entityOptions;

        public CustomerRepositoryTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _entityOptions = new DbContextOptionsBuilder<EntityContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Crud_EntityTest()
        {
            var edwardsUser = EdwardsUserMock.EdwardsUserModelFaker.Generate();

            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var entityContext = new EntityContext(_entityOptions);
            var unitOfWork = new UnitOfWork(entityContext);
            var dapperContext = new DapperContext(MockRepositoryBuilder.GetMockDbConnection().Object);
            var edwardsUserRepository = new EdwardsUserRepository(entityContext, dapperContext);

            edwardsUserRepository.Add(edwardsUser);
            var IsSaveCustomer = unitOfWork.Commit();

            edwardsUserRepository.Update(edwardsUser);
            var IsUpdateCustomer = unitOfWork.Commit();

            edwardsUserRepository.Remove(edwardsUser);
            var IsRemoverCustomer = unitOfWork.Commit();

            Assert.Equal(1, IsSaveCustomer);
            Assert.Equal(1, IsUpdateCustomer);
            Assert.Equal(1, IsRemoverCustomer);
        }
    }
}
