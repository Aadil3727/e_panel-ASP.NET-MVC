using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AdminServices.Interfaces.Admin;
using AdminServices.Mapper;
using AdminServices.Services.Admin;
using AdminServices.Interfaces.Category;
using AdminServices.Services.Category;
using AdminServices.Interfaces.Product;
using AdminServices.Services.Product;

namespace AdminServices.BL_dependency
{
    public static class BLDependency
    {
        public static void AddBLDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CateFileUpload>();
            services.AddScoped<FileUpload>();
            services.AddScoped<ProductMultiFileUpload>();
            services.AddScoped<ProductFileUpload>();
            services.AddHttpContextAccessor();
            services.AddScoped<CookieHandler.CookieHandler>();
            services.AddScoped<IAdmin, AuthServices>();
            services.AddScoped<Icategory,CategoryServices>();
            services.AddScoped<IProductService,ProductService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));

        }
    }
}
