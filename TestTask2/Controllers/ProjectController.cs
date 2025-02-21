using Microsoft.AspNetCore.Mvc;
using TestTask2.DTOs.Project;
using TestTask2.Helperls;
using TestTask2.Interfaces;
using TestTask2.Mappers;
using TestTask2.Models;
using TestTask2.Services;

namespace TestTask2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects([FromQuery] QueryObj queryObj)
    {
        return Ok(await _projectService.GetAllProjectsAsync(queryObj));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        return project == null ? NotFound() : Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var project = await _projectService.CreateProjectAsync(createProjectDto);
        return project == null ? StatusCode(500, "Не удалось сохранить проект") : Ok(project);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto updateProjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var project = await _projectService.UpdateProjectAsync(id, updateProjectDto);
        return project == null ? NotFound() : Ok(project);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _projectService.DeleteProjectAsync(id);
        return project == null ? NotFound() : Ok(project);
    }
    
}