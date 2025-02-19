using TestTask2.DTOs.Company;
using TestTask2.Models;

namespace TestTask2.Mappers;

public static class CompanyMapper
{
    public static CompanyDto ToCompanyDto(this Company company)
    {
        return new CompanyDto()
        {
            Id = company.Id,
            Name = company.Name,
        };
    }

    public static Company ToCompanyFromCompanyDto(this CreateCompanyDto companyDto)
    {
        return new Company()
        {
            Name = companyDto.Name,
        };
    }
}