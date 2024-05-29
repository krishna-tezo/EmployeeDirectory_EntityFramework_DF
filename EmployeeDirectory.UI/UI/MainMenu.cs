using EmployeeDirectory.Interfaces;

namespace EmployeeDirectory.UI
{
    public class MainMenu
    {

        private IEmployeeMenu employeeMenu;
        private IRoleMenu roleMenu;

        public MainMenu(IEmployeeMenu employeeMenu, IRoleMenu roleMenu)
        {
            this.employeeMenu = employeeMenu;
            this.roleMenu = roleMenu;
        }

        public void ShowMainMenu()
        {
            //Console.Clear();
            string? choice;
            bool showMenu = true;

            while (showMenu)
            {
                Console.WriteLine("\nMain Menu\n");
                Console.WriteLine("1. Employee Management\n2. Role Management\n3. Exit\n");
                Console.Write("\nChoose Any option:");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        employeeMenu.ShowEmployeeMenu();
                        break;
                    case "2":
                        roleMenu.ShowRoleMenu();
                        break;
                    case "3":
                        showMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Input! Please Re-Enter");
                        break;
                }
            }
        }
    }
}