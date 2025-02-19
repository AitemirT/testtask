using System.ComponentModel.DataAnnotations;

namespace TestTask2.DTOs.Project;

public class UpdateProjectDto
{
    [Required(ErrorMessage = "Название проекта обязательно к заполнению")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Длинна должна не должна быть меньше 3 и больше 100 символов")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Дата начала обязательна к заполнению")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Дата конца обязательна к заполнению")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Приоритет обязателен к заполнению")]
    [Range(1, 3)]
    public int Priority { get; set; }
    [Required]
    public int CustomerCompanyId { get; set; }
    [Required]
    public int ExecutorCompanyId { get; set; }
    [Required]
    public int ProjectManagerId { get; set; }
}