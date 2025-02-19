
using System.Text.Json.Serialization;

namespace TestTask2.Helperls;

public class QueryObj
{
    public int? Priority { get; set; }
    public string? ProjectName { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; } = false;

    
}