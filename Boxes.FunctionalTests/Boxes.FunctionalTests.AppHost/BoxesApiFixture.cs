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
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Boxes.FunctionalTests;

public sealed class BoxesApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost _app;

    public IResourceBuilder<PostgresServerResource> Postgres { get; private set; }
    private string _postgresConnectionString;

    public BoxesApiFixture()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(BoxesApiFixture).Assembly.FullName, DisableDashboard = true };
        var appBuilder = DistributedApplication.CreateBuilder(options);
        Postgres = appBuilder.AddPostgres("boxesservicedb");
        _app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { $"ConnectionStrings:{Postgres.Resource.Name}", _postgresConnectionString }
        });
        });
        builder.ConfigureServices(services =>
        {
            //services.AddSingleton<IStartupFilter>(new AutoAuthorizeStartupFilter());
        });
        return base.CreateHost(builder);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _app.StopAsync();
        if (_app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _app.Dispose();
        }
    }

    public async Task InitializeAsync()
    {
        await _app.StartAsync();
        _postgresConnectionString = await Postgres.Resource.GetConnectionStringAsync();
    }

    private class AutoAuthorizeStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                //builder.UseMiddleware<AutoAuthorizeMiddleware>();
                next(builder);
            };
        }
    }
}