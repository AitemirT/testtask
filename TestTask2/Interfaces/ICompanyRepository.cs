using TestTask2.DTOs.Company;
using TestTask2.Models;

namespace TestTask2.Interfaces;

public interface ICompanyRepository
{
    Task<List<Company>> GetAllCompaniesAsync();
    Task<Company?> GetCompanyByIdAsync(int id);
    Task<Company> CreateCompanyAsync(Company company);
    Task<Company?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto);
    Task<Company?> DeleteCompanyAsync(int id);
}