namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru actualizarea informatiilor unei companii.
/// Proprietatile sunt optionale si pot fi actualizate individual.
/// </summary>
public class CompanyUpdateDTO
{
    public string? Name { get; set; } // Nume nou (optional)
    public string? Description { get; set; } // Descriere noua (optional)
    public string? Location { get; set; } // Locatie noua (optional)
}