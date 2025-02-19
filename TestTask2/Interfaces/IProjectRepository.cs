using TestTask2.DTOs.Project;
using TestTask2.Helperls;
using TestTask2.Models;

namespace TestTask2.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllProjectsAsync(QueryObj queryObj);
    Task<Project?> GetProjectByIdAsync(int id);
    Task<Project> CreateProjectAsync(Project project);
    Task<Project?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
    Task<Project?> DeleteProjectAsync(int id);
}