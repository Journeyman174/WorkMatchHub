namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a transmite informatii complete despre o companie.
/// </summary>
public class CompanyDTO
{
    public Guid Id { get; set; } // Id-ul companiei
    public string Name { get; set; } = null!; // Numele companiei
    public string? Description { get; set; } // Descriere optionala
    public string? Location { get; set; } // Locatia companiei
    public UserDTO User { get; set; } = null!; // Utilizatorul asociat companiei
    public DateTime CreatedAt { get; set; } // Data crearii companiei
}