using EmployeeDirectory.Interfaces;

namespace EmployeeDirectory.UI.Menus
{
    public class EmployeeMenu : IEmployeeMenu
    {
        private readonly IUIService uiService;
        public EmployeeMenu(IUIService uiService)
        {
            this.uiService = uiService;
        }
        public void ShowEmployeeMenu()
        {
            Console.Clear();
            Console.WriteLine("\nWelcome to Employee Management\n");
            string? choice;
            bool showEmployeeMenu = true;

            while (showEmployeeMenu)
            {
                Console.WriteLine("\nEmployee Menu\n");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Display All Employees");
                Console.WriteLine("3. Display Single Employee");
                Console.WriteLine("4. Edit Employee");
                Console.WriteLine("5. Delete Employee");
                Console.WriteLine("6. Go Back");
                Console.Write("\nChoose Any option:");

                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        uiService.AddEmployee();
                        break;
                    case "2":
                        uiService.ViewEmployees();
                        break;
                    case "3":
                        uiService.ViewEmployee();
                        break;
                    case "4":
                        uiService.EditEmployee();
                        break;
                    case "5":
                        uiService.DeleteEmployee();
                        break;
                    case "6":
                        showEmployeeMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Input! Please Re-Enter");
                        break;
                }
            }
        }
    }
}