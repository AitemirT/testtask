using Microsoft.AspNetCore.Mvc;
using TestTask2.DTOs.Project;
using TestTask2.Helperls;
using TestTask2.Interfaces;
using TestTask2.Mappers;
using TestTask2.Models;

namespace TestTask2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects([FromQuery] QueryObj queryObj)
    {
        if (queryObj.StartDateFrom.HasValue)
        {
            queryObj.StartDateFrom = queryObj.StartDateFrom.Value.ToUniversalTime();
        }

        if (queryObj.StartDateTo.HasValue)
        {
            queryObj.StartDateTo = queryObj.StartDateTo.Value.ToUniversalTime();
        }
        var projects = await _projectRepository.GetAllProjectsAsync(queryObj);
        var projectDtos = projects.Select(p => p.ToProjectDto()).ToList();
        return Ok(projectDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProject(int id)
    {
        try
        {
            Project? project = await _projectRepository.GetProjectByIdAsync(id);
            return project is null ? NotFound() : Ok(project.ToProjectDto());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "Ошибка при получении проекта", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        createProjectDto.StartDate = createProjectDto.StartDate.ToUniversalTime();
        createProjectDto.EndDate = createProjectDto.EndDate.ToUniversalTime();
        var projectModel = createProjectDto.ToProjectFromCreateProjectDto();
        try
        {
            Project createdProject = await _projectRepository.CreateProjectAsync(projectModel);
            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject.ToProjectDto());
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);  
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto updateProjectDto)
    {
        updateProjectDto.StartDate = updateProjectDto.StartDate.ToUniversalTime();
        updateProjectDto.EndDate = updateProjectDto.EndDate.ToUniversalTime();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Project? project = await _projectRepository.UpdateProjectAsync(id, updateProjectDto);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project.ToProjectDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Project? project = await _projectRepository.DeleteProjectAsync(id);
        if(project == null) return NotFound();
        return NoContent();
    }
    
}