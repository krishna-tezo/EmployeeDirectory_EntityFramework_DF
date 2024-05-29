using EmployeeDirectory.Interfaces;

namespace EmployeeDirectory.UI.Menus
{
    public class RoleMenu : IRoleMenu
    {
        private readonly IUIService uiService;
        public RoleMenu(IUIService uiService)
        {
            this.uiService = uiService;
        }
        public void ShowRoleMenu()
        {
            Console.Clear();
            Console.WriteLine("\nWelcome to Role Management\n");
            string? choice;
            bool showRoleMenu = true;

            while (showRoleMenu)
            {
                Console.WriteLine("\nRole Menu\n");
                Console.WriteLine("1. Add Role");
                Console.WriteLine("2. Display All Roles");
                Console.WriteLine("3. Go Back");
                Console.Write("\nChoose Any option:");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        uiService.AddRole();

                        break;
                    case "2":
                        uiService.ViewAllRoles();

                        break;
                    case "3":
                        showRoleMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Input! Please Re-Enter");
                        break;
                }
            }
        }
    }
}
