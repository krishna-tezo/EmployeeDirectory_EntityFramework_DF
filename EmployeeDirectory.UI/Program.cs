using EmployeeDirectory.Core;
using EmployeeDirectory.UI;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeDirectory
{
    public static class Program
    {
        public static void Main()
        {
            IServiceCollection services = new ServiceCollection();
            StartupService startupService = new StartupService(services);

            ServiceProvider serviceProvider = startupService.Configure();
            StartApplication(serviceProvider);
        }
        public static void StartApplication(ServiceProvider serviceProvider)
        {
            MainMenu? menu = serviceProvider.GetService<MainMenu>();
            if (menu != null)
            {
                menu.ShowMainMenu();
            }
            else
            {
                Console.WriteLine("Some Error Occurred");
            }
        }
    }
}