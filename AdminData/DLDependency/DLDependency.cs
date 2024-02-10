using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Web_Admin.Data;

namespace AdminData.DLDependency
{
    public static class DLDependencyExtensions
    {
        public static void AddDLDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("web_admin")));

        }
    }
}
