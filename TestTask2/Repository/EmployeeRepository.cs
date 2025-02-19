using Microsoft.EntityFrameworkCore;
using TestTask2.Data;
using TestTask2.DTOs.Employee;
using TestTask2.Interfaces;
using TestTask2.Models;

namespace TestTask2.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Employee>> GetAllEmployeeAsync()
    {
        return await _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project).ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        var existingEmployee = _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .FirstOrDefault(e => e.Id == id);
        if(existingEmployee == null) return null;
        existingEmployee.FirstName = updateEmployeeDto.FirstName;
        existingEmployee.LastName = updateEmployeeDto.LastName;
        existingEmployee.Email = updateEmployeeDto.Email;
        existingEmployee.MiddleName = updateEmployeeDto.MiddleName;
        await _context.SaveChangesAsync();
        return existingEmployee;
    }

    public async Task<Employee?> DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null) return null;
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
}