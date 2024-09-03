using System.Net;

namespace MicroBoxer.Tests;

public class WebTests
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MicroBoxer_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("webfrontend");
        var response = await httpClient.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
