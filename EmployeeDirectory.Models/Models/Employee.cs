﻿namespace EmployeeDirectory.Models.Models;

public class Employee
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; }

    public DateOnly Dob { get; set; }

    public string MobileNumber { get; set; }

    public DateOnly JoinDate { get; set; }

    public string? ProjectId { get; set; }

    public string? RoleId { get; set; }

    public bool IsDeleted { get; set; }

}
