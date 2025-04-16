namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a transmite informatii despre o cerere de job.
/// </summary>
public class JobRequestDTO
{
    public Guid Id { get; set; } // Id-ul cererii de job
    public JobOfferDTO JobOffer { get; set; } = null!; // Oferta de job la care s-a aplicat
    public UserDTO User { get; set; } = null!; // Utilizatorul care a aplicat
    public string CoverLetter { get; set; } = null!; // Scrisoarea de intentie
    public DateTime CreatedAt { get; set; } // Data crearii cererii
}