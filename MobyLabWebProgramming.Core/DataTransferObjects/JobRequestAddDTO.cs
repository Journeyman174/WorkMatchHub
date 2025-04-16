namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a adauga o cerere de job.
/// Id-ul utilizatorului se obtine din token.
/// Jobul este identificat prin titlu si numele companiei.
/// </summary>
public class JobRequestAddDTO
{
    public string CoverLetter { get; set; } = null!; // Scrisoarea de intentie
    public string JobTitle { get; set; } = null!;    // Titlul jobului la care se aplica
    public string CompanyName { get; set; } = null!; // Numele companiei care a publicat jobul
}
