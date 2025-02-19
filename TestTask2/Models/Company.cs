namespace TestTask2.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Project> CustomerProjects { get; set; } = new List<Project>();
    public List<Project> ExecutorProjects { get; set; } = new List<Project>();
}