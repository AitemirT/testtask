using Microsoft.AspNetCore.Mvc;
using TestTask2.Interfaces;
using TestTask2.Models;

namespace TestTask2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectEmployeeController : ControllerBase
{
    private readonly IProjectEmployeeRepository _projectEmployeeRepository;

    public ProjectEmployeeController(IProjectEmployeeRepository projectEmployeeRepository)
    {
        _projectEmployeeRepository = projectEmployeeRepository;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddEmployeeToProject(int projectId, int employeeId)
    {
        if (projectId <= 0 || employeeId <= 0)
        {
            return BadRequest("Некорректный идентификатор проекта или сотрудника.");
        }

        try
        {
            if (await _projectEmployeeRepository.ProjectEmployeeExistsAsync(projectId, employeeId))
            {
                return BadRequest(new { Message = "Этот сотрудник уже добавлен в данный проект" });
            }

            await _projectEmployeeRepository.AddEmployeeToProjectAsync(projectId, employeeId);
            return Ok(new { Message = "Сотрудник успешно добавлен в проект." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Произошла ошибка не сервере", Details = ex.Message});
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeFromProject([FromQuery]int projectId, [FromQuery]int employeeId)
    {
        try
        {
            await _projectEmployeeRepository.RemoveEmployeeFromProjectAsync(projectId, employeeId);
            return Ok(new { Message = "Сотрудник успешно удален из проекта." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}