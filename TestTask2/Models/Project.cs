namespace TestTask2.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Priority { get; set; }
    public int CustomerCompanyId { get; set; }
    public int ExecutorCompanyId { get; set; }
    public int ProjectManagerId { get; set; }
    public Company CustomerCompany { get; set; }
    public Company ExecutorCompany { get; set; }
    public Employee ProjectManager { get; set; }
    public List<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    
}