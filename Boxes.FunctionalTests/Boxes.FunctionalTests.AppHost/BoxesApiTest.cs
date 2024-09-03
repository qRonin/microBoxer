using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.Testing;
using Asp.Versioning.Http;
using System.Net;
using Xunit;

namespace Boxes.FunctionalTests;

public sealed class BoxesApiTests : IClassFixture<BoxesApiFixture>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;

    public BoxesApiTests(BoxesApiFixture fixture)
    {
        var handler = new ApiVersionHandler(new QueryStringApiVersionWriter(), new ApiVersion(1.0));

        _webApplicationFactory = fixture;
        _httpClient = _webApplicationFactory.CreateDefaultClient(handler);
    }

    [Fact]
    public async Task GetAllStoredBoxesWorks()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/boxesapi/box/");
        var s = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
