using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using ToDo.Server.Tests.TestInfrastructure;
using Xunit;
using Xunit.Abstractions;

namespace ToDo.Server.Tests.Controllers;

[Collection(nameof(UserAccountApplicationFactoryCollection))]
public class AccountControllerTests 
{
    private readonly UserAccountApplicationFactory _fixture;
    private readonly ITestOutputHelper _output;
    private readonly Lazy<HttpClient> _httpClient;

    public AccountControllerTests(UserAccountApplicationFactory fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _httpClient = new Lazy<HttpClient>(() => {
            var client = fixture.CreateClient();
            return client;
        });
    }
    
    [Fact]
    public async Task ShouldCreateUser()
    {
        var response = await _httpClient.Value.PostAsJsonAsync("/Account/Register", new { username = "admin", password ="adminPassword", confirmPassword="adminPassword" });

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var testHelper = _fixture.Services.GetRequiredService<UserAccountTestHelper>();
        (await testHelper.GetUserAccounts()).Count(x => x.Username == "admin").ShouldBe(1);
    }
    
    [Fact]
    public async Task ShouldReturnValidationResultForIncorrectRequestWhenRegistering()
    {
        var response = await _httpClient.Value.PostAsJsonAsync("/Account/Register", new { });
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        var validationDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
        validationDetails!.Errors.Count.ShouldBe(3);
        validationDetails.Errors["UserName"][0].ShouldContain("The Username field is required.");
        validationDetails.Errors["Password"][0].ShouldContain("The Password field is required.");
        validationDetails.Errors["ConfirmPassword"][0].ShouldContain("The ConfirmPassword field is required.");
    }
    
    
    
    [Fact]
    public async Task ShouldBeAbleToLoginWithCorrectCredentials()
    {
        var testAccount = new TestUserAccount(Guid.NewGuid(), "guest", "guestPassword", DateTime.UtcNow);
        var testHelper = _fixture.Services.GetRequiredService<UserAccountTestHelper>();
        await testHelper.DeleteUserAccount(testAccount.Username);
        await testHelper.AddUserAccount(testAccount);
        
        var response = await _httpClient.Value.PostAsJsonAsync("/Account/Login", new { userName = testAccount.Username, password = testAccount.Password });
        
        _output.WriteLine(await response.Content.ReadAsStringAsync());
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    

    [Fact]
    public async Task ShouldReturnValidationErrorsWhenAttemptingToLogin()
    {
        var response = await _httpClient.Value.PostAsJsonAsync("/Account/Login", new { });
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        var validationDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
        validationDetails!.Errors.Count.ShouldBe(2);
        validationDetails.Errors["UserName"][0].ShouldContain("The Username field is required.");
        validationDetails.Errors["Password"][0].ShouldContain("The Password field is required.");
    }    
    
    [Fact]
    public async Task ShouldReturnBadRequestWhenUserDoesNotExistWhenAttemptingToLogin()
    {
        var response = await _httpClient.Value.PostAsJsonAsync("/Account/Login", new { userName = "random", password = "random" });
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
    }    

    
    
}