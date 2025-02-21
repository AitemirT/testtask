using TestTask2.Interfaces;
using TestTask2.Models;

namespace TestTask2.Services;

public class ProjectEmployeeService
{
    private readonly IProjectEmployeeRepository _projectEmployeeRepository;

    public ProjectEmployeeService(IProjectEmployeeRepository projectEmployeeRepository)
    {
        _projectEmployeeRepository = projectEmployeeRepository;
    }

    public async Task<bool> AddEmployeeToProjectAsync(int projectId, int employeeId)
    {
        if (await _projectEmployeeRepository.ProjectEmployeeExistsAsync(projectId, employeeId))
        {
            return false;
        }
        await _projectEmployeeRepository.AddEmployeeToProjectAsync(projectId, employeeId);
        return true;
    }
    public async Task<bool> RemoveEmployeeFromProjectAsync(int projectId, int employeeId)
    {
        await _projectEmployeeRepository.RemoveEmployeeFromProjectAsync(projectId, employeeId);
        return true;
    }
}