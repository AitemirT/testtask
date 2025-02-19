using System.ComponentModel.DataAnnotations;

namespace TestTask2.DTOs.Company;

public class UpdateCompanyDto
{
    [Required(ErrorMessage = "Название компании обязательно к заполнению")]
    public string Name { get; set; }
}