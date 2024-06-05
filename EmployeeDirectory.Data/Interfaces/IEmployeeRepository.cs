﻿using EmployeeDirectory.Data.Models;
using EmployeeDirectory.Data.SummaryModels;

namespace EmployeeDirectory.Data.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        List<EmployeeSummary> GetEmployeesSummary();
        EmployeeSummary GetEmployeeSummaryById(string id);
        int UpdateEmployee(Employee employee);
        int DeleteEmployee(string id);
    }
}