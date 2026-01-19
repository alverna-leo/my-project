using JobProviderApp.Helpers;
using JobProviderApp.Interface;
using JobProviderApp.Model;
using JobProviderApp.Repository;
using JobProviderApp.Service;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace JobProviderApp.Extension
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices
            (this IServiceCollection services, IConfiguration config)
        {
            //services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddScoped<ProtectedSessionStorage>();

            services.AddDbContext<JobProviderAppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IJobProviderRepository, JobProviderRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobservice, JobService>();

            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
