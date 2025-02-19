using Microsoft.AspNetCore.Mvc;
using TestTask2.DTOs.Employee;
using TestTask2.Interfaces;
using TestTask2.Mappers;
using TestTask2.Models;

namespace TestTask2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _employeeRepository.GetAllEmployeeAsync();
        var employeesDto = employees.Select(e => e.ToEmployeeDto());
        return Ok(employeesDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        try
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return employee is null ? NotFound() : Ok(employee.ToEmployeeDto());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "Ошибка при получении сотрудника", error = e.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto createEmployeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var employee = createEmployeeDto.ToEmployeeFromCreateEmployeeDto();
        Employee createdEmployee = await _employeeRepository.CreateEmployeeAsync(employee);
        if (createdEmployee is null) return StatusCode(500, "Не удалось сохранить сотрудника");
        return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee.ToEmployeeDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Employee? employee = await _employeeRepository.UpdateEmployeeAsync(id, updateEmployeeDto);
        if (employee is null) return NotFound();
        return Ok(employee.ToEmployeeDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Employee? employee = await _employeeRepository.DeleteEmployeeAsync(id);
        if (employee is null) return NotFound();
        return NoContent();
    }
}