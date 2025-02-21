using TestTask2.DTOs.Project;
using TestTask2.Helperls;
using TestTask2.Interfaces;
using TestTask2.Mappers;

namespace TestTask2.Services;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync(QueryObj queryObj)
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
        return projects.ConvertAll(p => p.ToProjectDto());
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int projectId)
    {
        var project = await _projectRepository.GetProjectByIdAsync(projectId);
        return project?.ToProjectDto();
    }

    public async Task<ProjectDto?> CreateProjectAsync(CreateProjectDto createProjectDto)
    {
        createProjectDto.StartDate = createProjectDto.StartDate.ToUniversalTime();
        createProjectDto.EndDate = createProjectDto.EndDate.ToUniversalTime();
        var project = createProjectDto.ToProjectFromCreateProjectDto();
        var createdProject = await _projectRepository.CreateProjectAsync(project);
        return createdProject?.ToProjectDto();
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
    {
        updateProjectDto.StartDate = updateProjectDto.StartDate.ToUniversalTime();
        updateProjectDto.EndDate = updateProjectDto.EndDate.ToUniversalTime();
        var project = await _projectRepository.UpdateProjectAsync(id, updateProjectDto);
        return project?.ToProjectDto();
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        var result = await _projectRepository.DeleteProjectAsync(projectId);
        return result != null;
    }
}