using Microsoft.EntityFrameworkCore;
using TestTask2.Data;
using TestTask2.Interfaces;
using TestTask2.Models;

namespace TestTask2.Repository;

public class ProjectEmployeeRepository : IProjectEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectEmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddEmployeeToProjectAsync(int projectId, int employeeId)
    {
        var existingProject = await _context.Projects
            .Include(p => p.CustomerCompany)
            .Include(p => p.ExecutorCompany)
            .Include(p => p.ProjectEmployees)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        
        var existingEmployee = _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .FirstOrDefault(e => e.Id == employeeId);
        
        if (existingProject == null || existingEmployee == null)
        {
            throw new ArgumentException("Проект или сотрудник не найден.");
        }
        
        var projectEmployee = new ProjectEmployee
        {
            ProjectId = projectId,
            EmployeeId = employeeId
        };
        await _context.ProjectEmployees.AddAsync(projectEmployee);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveEmployeeFromProjectAsync(int projectId, int employeeId)
    {
        var projectEmployee = await _context.ProjectEmployees
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

        if (projectEmployee == null)
        {
            throw new ArgumentException("Сотрудник не найден в данном проекте.");
        }

        _context.ProjectEmployees.Remove(projectEmployee);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ProjectEmployeeExistsAsync(int projectId, int employeeId)
    {
        return await _context.ProjectEmployees.AnyAsync(p => p.ProjectId == projectId && p.EmployeeId == employeeId);
    }
}