using Microsoft.AspNetCore.Mvc;
using TestTask2.DTOs.Employee;
using TestTask2.Interfaces;
using TestTask2.Mappers;
using TestTask2.Models;
using TestTask2.Services;

namespace TestTask2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        return Ok(await _employeeService.GetAllEmployeesAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto createEmployeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var employee = await _employeeService.CreateEmployeeAsync(createEmployeeDto);
        return employee == null ? StatusCode(500, "Не удалось создать сотрудника") : CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var employee = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeDto);
        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);
        return result ? NoContent() : NotFound();
    }
}