using TestTask2.DTOs.Employee;
using TestTask2.DTOs.Project;
using TestTask2.Models;

namespace TestTask2.Mappers;

public static class ProjectMapper
{
    public static ProjectDto ToProjectDto(this Project project)
    {
        return new ProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Priority = project.Priority,
            CustomerCompanyId = project.CustomerCompanyId,
            CustomerCompanyName = project.CustomerCompany?.Name ?? string.Empty,
            ExecutorCompanyId = project.ExecutorCompanyId,
            ExecutorCompanyName = project.ExecutorCompany?.Name ?? string.Empty,
            ProjectManagerId = project.ProjectManagerId,
            ProjectManagerName = project?.ProjectManager != null ? $"{project.ProjectManager.LastName} {project.ProjectManager.FirstName} {project.ProjectManager.MiddleName}" : string.Empty,
            ProjectEmployees = project?.ProjectEmployees
                .Where(pe => pe.Employee != null)
                .Select(pe => new EmployeeDto()
                {
                    Id = pe.Employee.Id,
                    FirstName = pe.Employee.FirstName,
                    LastName = pe.Employee.LastName,
                }).ToList()
        };
    }

    public static Project ToProjectFromCreateProjectDto(this CreateProjectDto projectDto)
    {
        return new Project
        {
            Name = projectDto.Name,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            Priority = projectDto.Priority,
            CustomerCompanyId = projectDto.CustomerCompanyId,
            ExecutorCompanyId = projectDto.ExecutorCompanyId,
            ProjectManagerId = projectDto.ProjectManagerId,
        };
    }
    
}