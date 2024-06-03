using EmployeeDirectory.Interfaces;
using EmployeeDirectory.UI.Menus;
using EmployeeDirectory.UI.UIServices;
using EmployeeDirectory.UI;
using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory.Services;
using EmployeeDirectory.Controllers;
using EmployeeDirectory.UI.Controllers;
using Microsoft.Extensions.Configuration;
using EmployeeDirectory.Data.Services;
using EmployeeDirectory.Data.Data.Services;
using EmployeeDirectory.UI.Interfaces;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Models.Models;

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
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("MyDBConnectionString")));
            services.AddSingleton<ICommonDataService , CommonDataService>();
            services.AddSingleton<IEmployeeDataService , EmployeeDataService>();
            services.AddSingleton<IRoleDataService, RoleDataService>();
            services.AddSingleton<ICommonServices, CommonServices>(); //remove this
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<IUIService, UIService>();
            services.AddSingleton<IEmployeeMenu, EmployeeMenu>();
            services.AddSingleton<IRoleMenu, RoleMenu>();
            services.AddSingleton<IValidator, Validator>();
            services.AddSingleton<IEmployeeController, EmployeeController>();
            services.AddSingleton<IRoleController, RoleController>();
            services.AddSingleton<ICommonController, CommonController>(); //remove this
            services.AddSingleton<MainMenu>();
            

            return services.BuildServiceProvider();
        }
    }
}