using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Configuration;
using Testcontainers.PostgreSql;

namespace ElyApi.IntegrationTests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
    {
        //private readonly PostgreSqlContainer _container = new PostgreSqlBuilder();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services => 
            {
                var descriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<NumbersContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<NumbersContext>(options =>
                {
                    options
                        .UseNpgsql("");
                });
            });
        }
    }
}
