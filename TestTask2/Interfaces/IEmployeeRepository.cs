using TestTask2.DTOs.Employee;
using TestTask2.Models;

namespace TestTask2.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployeeAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<Employee?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);
    Task<Employee?> DeleteEmployeeAsync(int id);
}