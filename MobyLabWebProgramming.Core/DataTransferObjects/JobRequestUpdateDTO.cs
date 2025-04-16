namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a actualiza o cerere de job.
/// Actualizarea este identificata prin titlul jobului, numele companiei si utilizatorul autentificat (din token).
/// </summary>
public class JobRequestUpdateDTO
{
    public string JobTitle { get; set; } = null!;    // Titlul jobului pentru care s-a aplicat
    public string CompanyName { get; set; } = null!; // Numele companiei care a publicat jobul
    public string CoverLetter { get; set; } = null!; // Noua scrisoare de intentie
}
