using TestTask2.DTOs.Employee;
using TestTask2.Models;

namespace TestTask2.Mappers;

public static class EmployeeMapper
{
    public static EmployeeDto ToEmployeeDto(this Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            MiddleName = employee.MiddleName,
        };
    }

    public static Employee ToEmployeeFromCreateEmployeeDto(this CreateEmployeeDto employeeDto)
    {
        return new Employee()
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            MiddleName = employeeDto.MiddleName,
        };
    }
}