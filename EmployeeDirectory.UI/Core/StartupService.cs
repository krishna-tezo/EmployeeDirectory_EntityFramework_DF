using EmployeeDirectory.Interfaces;
using EmployeeDirectory.UI.Menus;
using EmployeeDirectory.UI.UIServices;
using EmployeeDirectory.UI;
using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory.Services;
using EmployeeDirectory.Controllers;
using EmployeeDirectory.UI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Data.Models;
using EmployeeDirectory.Data.Interfaces;
using EmployeeDirectory.Data.Repositories;
using EmployeeDirectory.Models.Interfaces;
using EmployeeDirectory.UI.Models;

namespace EmployeeDirectory.Core
{
    public class StartupService
    {
        private IServiceCollection services;
        private IConfiguration configuration;
        public StartupService(IServiceCollection services)
        {
            this.services = services;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
           

            configuration = builder.Build();
            
        }

        public ServiceProvider Configure()
        {
            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyDBConnectionString"))
                       .EnableSensitiveDataLogging();
            });
            services.AddAutoMapper(typeof(Data.Models.MappingProfile));
            services.AddAutoMapper(typeof(UI.Models.MappingProfile));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUIService, UIService>();
            services.AddScoped<IEmployeeMenu, EmployeeMenu>();
            services.AddScoped<IRoleMenu, RoleMenu>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IEmployeeController, EmployeeController>();
            services.AddScoped<IRoleController, RoleController>();
            services.AddScoped<MainMenu>();
            

            return services.BuildServiceProvider();
        }
    }
}