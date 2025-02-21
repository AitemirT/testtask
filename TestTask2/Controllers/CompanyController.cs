using Microsoft.AspNetCore.Mvc;
using TestTask2.DTOs.Company;
using TestTask2.Interfaces;
using TestTask2.Mappers;
using TestTask2.Models;
using TestTask2.Services;

namespace TestTask2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _companyService;
    public CompanyController(CompanyService companyService)
    {
       _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        var companies = await _companyService.GetAllCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCompanyById(int id)
    {
        var company = await _companyService.GetCompanyByIdAsync(id);
        return company == null ? NotFound() : Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto companyDto)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        var company = await _companyService.CreateCompanyAsync(companyDto);
        return company == null ? StatusCode(500, "Не удалось сохранить компанию") : CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDto companyDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var company = await _companyService.UpdateCompanyAsync(id, companyDto);
        return company == null ? NotFound() : Ok(company);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var result = await _companyService.DeleteEmployeeAsync(id);
        return result ? NoContent() : NotFound();
    }
}