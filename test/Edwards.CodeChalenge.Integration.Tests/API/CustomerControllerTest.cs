using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Edwards.CodeChalenge.API;
using Edwards.CodeChalenge.Core.Tests.Mocks;
using Edwards.CodeChalenge.Core.Tests.Mocks.Factory;
 
using Xunit;
using System.Collections.Generic;
using Dapper;
using System.Text;
using Moq.Dapper;
using Edwards.CodeChalenge.Core.Tests.Fixture;
using Edwards.CodeChalenge.Domain.Models;
using System.Text.Json;

namespace Edwards.CodeChalenge.Integration.Tests.API;

public class EdwardsUserControllerTest : IClassFixture<WebApplicationFixture<Startup>>
{
    private readonly HttpClient _httpClient;

    public EdwardsUserControllerTest(WebApplicationFixture<Startup> factory)
    {
        _httpClient = factory.CreateClient();
        this.Arrange();
        SqlMapper.PurgeQueryCache();
    }

    private void Arrange()
    {
        var queryEdwardsUsers = @"SELECT Id , FirstName, LastName, Email, Notes, .DateCreated 
                            FROM dbo.EdwardsUser ";

        List<EdwardsUser> edwardsUsers = EdwardsUserMock.EdwardsUserModelFaker.Generate(6);

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUsers,
            null,
            null,
            null,
            null)).ReturnsAsync(edwardsUsers);


        var queryEdwardsUserByName = @"SELECT Id , FirstName, LastName, Email, Notes, .DateCreated
                          FROM dbo.EdwardsUser 
                          WHERE FirstName = @Name";

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUserByName,
            It.IsAny<object>(),
            null,
            null,
            null)).ReturnsAsync(() => new List<EdwardsUser>());



        var queryEdwardsUserById = @"SELECT Id , FirstName, LastName, Email, Notes, .DateCreated
                          FROM dbo.EdwardsUser c
                          WHERE c.Id = @Id";

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUserById,
            It.IsAny<object>(),
            null,
            null,
            null)).ReturnsAsync(new List<EdwardsUser>());

    }

    #region [ 200 Ok Test ]
    [Fact]
    public async Task GetAll_OKTestAsync()
    {
        MockAuthorizationFactory.AddAdminHeaders(_httpClient);

        var response = await _httpClient.GetAsync("/api/v1/edwardsUsers");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task GetById_OKTestAsync()
    {
        var queryEdwardsUserById = @"SELECT c.Id, a.Id AS Id, c.Name, c.DateCreated, a.CEP
                          FROM dbo.EdwardsUser c
                          INNER JOIN dbo. a
                          ON c.Id = a.Id
                          WHERE c.Id = @Id";

        List<EdwardsUser> edwardsUserById = EdwardsUserMock.EdwardsUserModelFaker.Generate(1);

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUserById,
            It.IsAny<object>(),
            null,
            null,
            null)).ReturnsAsync(edwardsUserById);

        MockAuthorizationFactory.AddAdminHeaders(_httpClient);

        var response = await _httpClient.GetAsync("/api/v1/edwardsUsers/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetByName_OKTestAsync()
    {
        var queryEdwardsUserByName = @"SELECT c.Id AS id, a.Id AS addressId, c.Name AS name, c.DateCreated AS dateCreated, a.CEP AS cep
                          FROM dbo.EdwardsUser c
                          INNER JOIN dbo. a
                          ON c.addressId = a.Id
                          WHERE c.Name = @Name";

        List<EdwardsUser> edwardsUserByName = EdwardsUserMock.EdwardsUserModelFaker.Generate(1);

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUserByName,
            It.IsAny<object>(),
            null,
            null,
            null)).ReturnsAsync(edwardsUserByName);

        MockAuthorizationFactory.AddAdminHeaders(_httpClient);

        var response = await _httpClient.GetAsync("/api/v1/edwardsUsers/name/joao");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region [ 201 Created Test ]

    [Fact]
    public async Task PostEdwardsUser_CreatedTestAsync()
    {
        MockAuthorizationFactory.AddAdminHeaders(_httpClient);
        var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();
        edwardsUser.Id = 0;

        var response = await _httpClient.PostAsync("/api/v1/edwardsUsers",
            new StringContent(JsonSerializer.Serialize(edwardsUser),
            Encoding.UTF8,
            "application/json"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    #endregion

    #region [ 202 Accepted Test ]

    [Fact]
    public async Task PutEdwardsUser_AcceptedTestAsync()
    {
        var queryEdwardsUserById = @"SELECT Id , FirstName, LastName, Email, Notes, .DateCreated
                          FROM dbo.EdwardsUser c
                          WHERE c.Id = @Id";

        List<EdwardsUser> edwardsUserModel = EdwardsUserMock.EdwardsUserModelFaker.Generate(1);

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUserById,
            It.IsAny<object>(),
            null,
            null,
            null)).ReturnsAsync(edwardsUserModel);

        MockAuthorizationFactory.AddAdminHeaders(_httpClient);

        var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();
        edwardsUser.Id = edwardsUserModel[0].Id;

        var response = await _httpClient.PutAsync($"/api/v1/edwardsUsers/{edwardsUserModel[0].Id}",
            new StringContent(JsonSerializer.Serialize(edwardsUser),
            Encoding.UTF8,
            "application/json"));


        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }


    [Fact]
    public async Task DeleteEdwardsUser_AcceptedTestAsync()
    {
        var queryEdwardsUserById = @"SELECT Id , FirstName, LastName, Email, Notes, .DateCreated
                          FROM dbo.EdwardsUser c
                          WHERE c.Id = @Id";

        List<EdwardsUser> edwardsUserModel = EdwardsUserMock.EdwardsUserModelFaker.Generate(1);

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUserById,
            It.IsAny<object>(),
            null,
            null,
            null)).ReturnsAsync(edwardsUserModel);

        MockAuthorizationFactory.AddAdminHeaders(_httpClient);

        var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();
        edwardsUser.Id = edwardsUserModel[0].Id;

        var response = await _httpClient.DeleteAsync($"/api/v1/edwardsUsers/{edwardsUserModel[0].Id}");

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    #endregion

    #region [ 204 NoContent Test ]

    [Theory]
    [InlineData("/api/v1/edwardsUsers/1", "Put")]
    public async Task Put_NoContentTestAsync(string endpoint, string method)
    {
        MockAuthorizationFactory.AddAdminHeaders(this._httpClient);

        var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();
        edwardsUser.Id = 1;

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = new System.Uri($"{this._httpClient.BaseAddress.OriginalString}{endpoint}"),
            Method = new HttpMethod(method),
            Headers = { { "User-Agent", "csharp" } },
            Content = new StringContent(JsonSerializer.Serialize(edwardsUser),
                Encoding.UTF8,
            "application/json")
        };

        var response = await this._httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


    [Theory]
    [InlineData("/api/v1/edwardsUsers/1", "Get")]
    [InlineData("/api/v1/edwardsUsers/name/joao", "Get")]
    [InlineData("/api/v1/edwardsUsers", "Post")]
    [InlineData("/api/v1/edwardsUsers/1", "Delete")]
    public async Task Generic_NoContentTestAsync(string endpoint, string method)
    {
        MockAuthorizationFactory.AddAdminHeaders(this._httpClient);

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = new System.Uri($"{this._httpClient.BaseAddress.OriginalString}{endpoint}"),
            Method = new HttpMethod(method),
            Headers = { { "User-Agent", "csharp" } },
            Content = new StringContent("{}",
                Encoding.UTF8,
                "application/json")
        };

        var response = await this._httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

        string message = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    #endregion

    #region [ 400 BadRequest Test ]

    [Fact]
    public async Task PostEdwardsUser_BadRequesTestAsync()
    {
        var queryEdwardsUserByName = @"SELECT Id , FirstName, LastName, Email, Notes, .DateCreated
                          FROM dbo.EdwardsUser c                         
                          WHERE c.FirstName = @Name";

        List<EdwardsUser> edwardsUserByName = EdwardsUserMock.EdwardsUserModelFaker.Generate(1);

        MockRepositoryBuilder.GetMockDbConnection().SetupDapperAsync(c => c.QueryAsync<EdwardsUser>(
            queryEdwardsUserByName,
            It.IsAny<object>(),
            null,
            null,
            null)).ReturnsAsync(edwardsUserByName);

        MockAuthorizationFactory.AddAdminHeaders(_httpClient);
        var edwardsUser = EdwardsUserMock.EdwardsUserViewModelFaker.Generate();
        edwardsUser.Id = edwardsUserByName[0].Id;
        edwardsUser.FirstName = edwardsUserByName[0].FirstName;

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = new System.Uri($"{this._httpClient.BaseAddress.OriginalString}/api/v1/edwardsUsers"),
            Method = new HttpMethod("Post"),
            Headers = { { "User-Agent", "csharp" } },
            Content = new StringContent(JsonSerializer.Serialize(edwardsUser),
                Encoding.UTF8,
                "application/json")
        };

        var response = await this._httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);


        string message = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Theory]
    [InlineData("/api/v1/edwardsUsers/a", "Get")]
    [InlineData("/api/v1/edwardsUsers", "Post")]
    [InlineData("/api/v1/edwardsUsers/1", "Put")]
    [InlineData("/api/v1/edwardsUsers/a", "Delete")]
    public async Task Generic_BadRequestTestAsync(string endpoint, string method)
    {
        MockAuthorizationFactory.AddAdminHeaders(this._httpClient);

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = new System.Uri($"{this._httpClient.BaseAddress.OriginalString}{endpoint}"),
            Method = new HttpMethod(method),
            Headers = { { "User-Agent", "csharp" } },
            Content = new StringContent("",
                Encoding.UTF8,
                "application/text")
        };

        var response = await this._httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

        string message = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region [ 401 Unauthorized Test ]
    /*

    [Theory]
    [InlineData("/api/v1/edwardsUsers", "Get")]
    [InlineData("/api/v1/edwardsUsers/1", "Get")]
    [InlineData("/api/v1/edwardsUsers/name/joao", "Get")]
    [InlineData("/api/v1/edwardsUsers", "Post")]
    [InlineData("/api/v1/edwardsUsers/1", "Put")]
    [InlineData("/api/v1/edwardsUsers/1", "Delete")]
    public async Task Generic_UnauthorizedTestAsync(string endpoint, string method)
    {
        MockAuthorizationFactory.AddAnonymousHeaders(this._httpClient);

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = new System.Uri($"{this._httpClient.Base.OriginalString}{endpoint}"),
            Method = new HttpMethod(method),
            Headers = { { "User-Agent", "csharp" } },
            Content = new StringContent("{}",
                Encoding.UTF8,
                "application/json")
        };

        var response = await this._httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    */

    #endregion
}
