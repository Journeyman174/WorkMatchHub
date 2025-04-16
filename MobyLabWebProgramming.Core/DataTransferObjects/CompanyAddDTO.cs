namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a adauga o companie.
/// </summary>
public class CompanyAddDTO
{
    public string Name { get; set; } = null!; // Numele companiei
    public string? Description { get; set; } // Descriere optionala
    public string? Location { get; set; } // Locatia companiei
}