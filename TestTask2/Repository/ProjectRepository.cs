using Microsoft.EntityFrameworkCore;
using TestTask2.Data;
using TestTask2.DTOs.Project;
using TestTask2.Helperls;
using TestTask2.Interfaces;
using TestTask2.Models;

namespace TestTask2.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllProjectsAsync(QueryObj queryObj)
    {
        var projects = _context.Projects
            .Include(p => p.CustomerCompany)
            .Include(p => p.ExecutorCompany)
            .Include(p => p.ProjectManager)
            .Include(p => p.ProjectEmployees)
            .AsQueryable();

        if (queryObj.Priority.HasValue)
        {
            projects = projects.Where(p => p.Priority == queryObj.Priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryObj.ProjectName))
        {
            projects = projects.Where(p => p.Name.ToLower().Contains(queryObj.ProjectName.ToLower()));
        }

        if (queryObj.StartDateFrom.HasValue)
        {
            projects = projects.Where(p => p.StartDate >= queryObj.StartDateFrom);
        }

        if (queryObj.StartDateTo.HasValue)
        {
            projects = projects.Where(p => p.StartDate <= queryObj.StartDateTo);
        }

        if (!string.IsNullOrWhiteSpace(queryObj.SortBy))
        {
            projects = queryObj.IsDescending 
                ? projects.OrderByDescending(p => EF.Property<object>(p, queryObj.SortBy))
                : projects.OrderBy(p => EF.Property<object>(p, queryObj.SortBy));
        }
        return await projects.ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.CustomerCompany)
            .Include(p => p.ExecutorCompany)
            .Include(p => p.ProjectManager)
            .Include(p => p.ProjectEmployees)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        var customerCompanyExists = await _context.Companies.AnyAsync(c => c.Id == project.CustomerCompanyId);
        if (!customerCompanyExists)
        {
            throw new ArgumentException("Компания заказчика не существует.");
        }

        var executorCompanyExists = await _context.Companies.AnyAsync(c => c.Id == project.ExecutorCompanyId);
        if (!executorCompanyExists)
        {
            throw new ArgumentException("Компания исполнителя не существует.");
        }

        var projectManagerExists = await _context.Employees.AnyAsync(e => e.Id == project.ProjectManagerId);
        if (!projectManagerExists)
        {
            throw new ArgumentException("Такого руководителя не сущствует не существует.");
        }
        
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        
        var proj = await _context.Projects
            .Include(p => p.CustomerCompany)
            .Include(p => p.ExecutorCompany)
            .Include(p => p.ProjectManager)
            .FirstOrDefaultAsync(p => p.Id == project.Id);
        return proj;
    }

    public async Task<Project?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
    {
        var existingProject = await _context.Projects
            .Include(p => p.CustomerCompany)
            .Include(p => p.ExecutorCompany)
            .Include(p => p.ProjectEmployees)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (existingProject == null)
        {
            return null;
        }
        existingProject.Name = updateProjectDto.Name;
        existingProject.StartDate = updateProjectDto.StartDate;
        existingProject.EndDate = updateProjectDto.EndDate;
        existingProject.Priority = updateProjectDto.Priority;
        existingProject.CustomerCompanyId = updateProjectDto.CustomerCompanyId;
        existingProject.ExecutorCompanyId = updateProjectDto.ExecutorCompanyId;
        existingProject.ProjectManagerId = updateProjectDto.ProjectManagerId;
        await _context.SaveChangesAsync();
        return existingProject;
    }

    public async Task<Project?> DeleteProjectAsync(int id)
    {
       var project = await _context.Projects
           .Include(p => p.CustomerCompany)
           .Include(p => p.ExecutorCompany)
           .Include(p => p.ProjectEmployees)
           .FirstOrDefaultAsync(p => p.Id == id);
       if(project == null) return null;
       _context.ProjectEmployees.RemoveRange(project.ProjectEmployees);
       _context.Projects.Remove(project);
       await _context.SaveChangesAsync();
       return project;
       
    }
}