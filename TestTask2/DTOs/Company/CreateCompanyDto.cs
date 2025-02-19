using System.ComponentModel.DataAnnotations;

namespace TestTask2.DTOs.Company;

public class CreateCompanyDto
{
    [Required(ErrorMessage = "Название компании обязательно к заполнению")]
    public string Name { get; set; }
}