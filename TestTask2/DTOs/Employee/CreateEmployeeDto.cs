using System.ComponentModel.DataAnnotations;

namespace TestTask2.DTOs.Employee;

public class CreateEmployeeDto
{
    [Required(ErrorMessage = "Имя обязательно к заполнению")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Фамилия обязательна к заполнению")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Отчество обязательно к заполнению")]
    public string MiddleName { get; set; }
    [Required(ErrorMessage = "Почта обязательна к заполнению")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; }
}