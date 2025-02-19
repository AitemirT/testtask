using System.ComponentModel.DataAnnotations;

namespace TestTask2.DTOs.Company;

public class CompanyDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Название компании обязательно к заполнению")]
    public string Name { get; set; }
}