using TestTask2.DTOs.Employee;
using TestTask2.Interfaces;
using TestTask2.Mappers;

namespace TestTask2.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllEmployeeAsync();
        return employees.ConvertAll(e => e.ToEmployeeDto());
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        return employee?.ToEmployeeDto();
    }

    public async Task<EmployeeDto?> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        var employee = createEmployeeDto.ToEmployeeFromCreateEmployeeDto();
        var createdEmployee = await _employeeRepository.CreateEmployeeAsync(employee);
        return createdEmployee?.ToEmployeeDto();
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _employeeRepository.UpdateEmployeeAsync(id, updateEmployeeDto);
        return employee?.ToEmployeeDto();
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var deletedEmployee = await _employeeRepository.DeleteEmployeeAsync(id);
        return deletedEmployee != null;
    }
}