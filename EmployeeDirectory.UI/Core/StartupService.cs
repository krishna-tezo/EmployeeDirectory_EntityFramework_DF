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
using EmployeeDirectory.Data;
using EmployeeDirectory.Data.Data.Services;
using EmployeeDirectory.UI.Interfaces;

namespace EmployeeDirectory.Core
{
    public class StartupService
    {
        private IServiceCollection services;
        public StartupService(IServiceCollection services)
        {
            this.services = services;
        }

        public ServiceProvider Configure()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            string connectionString = configBuilder.GetSection("ConnectionStrings")["MyDBConnectionString"];
            if(connectionString != null)
            {
                services.AddScoped<IDbConnection> (db=> new DbConnection(connectionString));
            }
            else
            {
                throw new Exception("Error");
            }
            services.AddSingleton<ICommonDataService , CommonDataService>();
            services.AddSingleton<IEmployeeDataService , EmployeeDataService>();
            services.AddSingleton<IRoleDataService, RoleDataService>();
            services.AddSingleton<IDataServiceManager, DataServiceManager>();
            services.AddSingleton<ICommonServices, CommonServices>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<IUIService, UIService>();
            services.AddSingleton<IEmployeeMenu, EmployeeMenu>();
            services.AddSingleton<IRoleMenu, RoleMenu>();
            services.AddSingleton<IValidator, Validator>();
            services.AddSingleton<IEmployeeController, EmployeeController>();
            services.AddSingleton<IRoleController, RoleController>();
            services.AddSingleton<ICommonController, CommonController>();
            services.AddSingleton<MainMenu>();
            

            return services.BuildServiceProvider();
        }
    }
}