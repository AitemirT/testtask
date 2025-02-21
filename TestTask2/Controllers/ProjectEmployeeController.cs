using Microsoft.AspNetCore.Mvc;
using TestTask2.Interfaces;
using TestTask2.Models;
using TestTask2.Services;

namespace TestTask2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectEmployeeController : ControllerBase
{
    private readonly ProjectEmployeeService _projectEmployeeService;

    public ProjectEmployeeController(ProjectEmployeeService projectEmployeeService)
    {
        _projectEmployeeService = projectEmployeeService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddEmployeeToProject([FromQuery] int projectId, [FromQuery] int employeeId)
    {
        if (projectId <= 0 || employeeId <= 0)
        {
            return BadRequest("Некорректный идентификатор проекта или сотрудника.");
        }
        var result = await _projectEmployeeService.AddEmployeeToProjectAsync(projectId, employeeId);
        return result ? Ok(new { Message = "Сотрудник успешно добавлен в проект." }) : BadRequest(new { Message = "Этот сотрудник уже добавлен в данный проект" });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeFromProject([FromQuery]int projectId, [FromQuery]int employeeId)
    {
        var result = await _projectEmployeeService.RemoveEmployeeFromProjectAsync(projectId, employeeId);
        return result ? Ok(new { Message = "Сотрудник успешно удален из проекта." }) : BadRequest(new { Message = "Ошибка удаления" });
    }
}