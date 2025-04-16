namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a declansa adaugarea unui job salvat.
/// Jobul este identificat prin titlu si numele companiei.
/// </summary>
public class SavedJobAddDTO
{
    public string JobTitle { get; set; } = null!;     // Titlul jobului
    public string CompanyName { get; set; } = null!;  // Numele companiei
}
