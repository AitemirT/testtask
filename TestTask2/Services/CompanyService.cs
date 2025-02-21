using TestTask2.DTOs.Company;
using TestTask2.Interfaces;
using TestTask2.Mappers;
using TestTask2.Models;

namespace TestTask2.Services;

public class CompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<List<CompanyDto>> GetAllCompaniesAsync()
    {
        var companies = await _companyRepository.GetAllCompaniesAsync();
        return companies.ConvertAll(c => c.ToCompanyDto());
    }

    public async Task<CompanyDto?> GetCompanyByIdAsync(int companyId)
    {
        var company = await _companyRepository.GetCompanyByIdAsync(companyId);
        return company?.ToCompanyDto();
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
    {
        var company = createCompanyDto.ToCompanyFromCompanyDto();
        var createdCompany = await _companyRepository.CreateCompanyAsync(company);
        return createdCompany.ToCompanyDto();
    }

    public async Task<CompanyDto?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
    {
        var company = await _companyRepository.UpdateCompanyAsync(id, updateCompanyDto);
        return company?.ToCompanyDto();
    }
    
    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _companyRepository.DeleteCompanyAsync(id);
        return employee != null;
    }
}