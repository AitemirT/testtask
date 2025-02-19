using Microsoft.AspNetCore.Mvc;
using TestTask2.DTOs.Company;
using TestTask2.Interfaces;
using TestTask2.Mappers;
using TestTask2.Models;

namespace TestTask2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyController(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        var companies = await _companyRepository.GetAllCompaniesAsync();
        var companiesDto = companies.Select(c => c.ToCompanyDto());
        return Ok(companiesDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCompanyById(int id)
    {
        try
        {
            Company? company = await _companyRepository.GetCompanyByIdAsync(id);
            return company is null ? NotFound() : Ok(company.ToCompanyDto());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "Ошибка при получении компании", error = e.Message });
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto companyDto)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        var company = companyDto.ToCompanyFromCompanyDto();
        Company? createdCompany = await _companyRepository.CreateCompanyAsync(company);
        if (createdCompany is null) return StatusCode(500, "Не удалось сохранить компанию");
        return CreatedAtAction(nameof(GetCompanyById), new { createdCompany.Id }, createdCompany.ToCompanyDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDto companyDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Company? company = await _companyRepository.UpdateCompanyAsync(id, companyDto);
        if (company is null) return NotFound();
        return Ok(company.ToCompanyDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Company? company = await _companyRepository.DeleteCompanyAsync(id);
        if (company is null) return NotFound();
        return NoContent();
    }
}