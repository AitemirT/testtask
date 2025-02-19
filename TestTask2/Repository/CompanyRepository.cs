using Microsoft.EntityFrameworkCore;
using TestTask2.Data;
using TestTask2.DTOs.Company;
using TestTask2.Interfaces;
using TestTask2.Models;

namespace TestTask2.Repository;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Company>> GetAllCompaniesAsync()
    {
        return await _context.Companies
            .Include(c => c.CustomerProjects)
            .Include(c => c.ExecutorProjects)
            .ToListAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        return await _context.Companies
            .Include(c => c.CustomerProjects)
            .Include(c => c.ExecutorProjects)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Company> CreateCompanyAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Company?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
    {
        var existingCompany = _context.Companies
            .Include(c => c.CustomerProjects)
            .Include(c => c.ExecutorProjects)
            .FirstOrDefault(c => c.Id == id);
        if(existingCompany == null) return null;
        existingCompany.Name = updateCompanyDto.Name;
        await _context.SaveChangesAsync();
        return existingCompany;
    }

    public async Task<Company?> DeleteCompanyAsync(int id)
    {
       var company = _context.Companies
           .Include(c => c.CustomerProjects)
           .Include(c => c.ExecutorProjects)
           .FirstOrDefault(c => c.Id == id);
       if(company == null) return null;
       _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return company;
    }
}