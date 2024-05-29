﻿using EmployeeDirectory.Models;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.UI.ViewModels;
using EmployeeDirectory.ViewModel;

namespace EmployeeDirectory.Interfaces
{
    public interface IUIService
    {
        void AddEmployee();
        void AddRole();
        void DeleteEmployee();
        void EditEmployee();
        Employee GetEmployeeDetailsFromConsole(Employee employee, EmployeeFormType formType, string empId = "");
        
        void ShowEmployeesDataInTabularFormat(List<EmployeeView> employees);
        void ShowRolesDataInTabularFormat(List<RoleView> roles);
        void ViewAllRoles();
        void ViewEmployee();
        void ViewEmployees();
    }
}