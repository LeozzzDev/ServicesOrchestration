using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CulDeSacServicesOrchestration.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Tynamix.ObjectFiller;
using Xunit;

namespace CulDeSacServicesOrchestration.IntegrationTests;

public class StudentsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public StudentsControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetStatus_ReturnsOk()
    {
        var response = await _httpClient.GetAsync("/status");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/api")]
    [InlineData("/api/student")]
    public async Task GetEndpointNotExists_ReturnsNotFound(string url)
    {
        var response = await _httpClient.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnNewRegisteredStudent()
    {
        HttpRequestMessage message = GetHttpRequestMessage();
        var response = await _httpClient.SendAsync(message);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEmpty(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task SholudReturnStudents()
    {
        var response = await _httpClient.GetAsync("api/students");
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(await response.Content.ReadAsStringAsync());
    }

    private HttpRequestMessage GetHttpRequestMessage()
    {
        Student student = GetRandomStudent();
        
        StringContent content = 
            new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");

        return new HttpRequestMessage
        {
            RequestUri = new Uri("http://localhost/api/students"),
            Method = HttpMethod.Post,
            Content = content
        };
    }

    private Student GetRandomStudent()
        => new Filler<Student>().Create();
}