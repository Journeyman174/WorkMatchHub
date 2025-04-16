namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a transmite informatii despre un job salvat.
/// </summary>
public class SavedJobDTO
{
    public Guid Id { get; set; } // Id-ul jobului salvat
    public JobOfferDTO JobOffer { get; set; } = default!; // Oferta de job asociata
    public UserDTO User { get; set; } = default!; // Utilizatorul care a salvat jobul
    public DateTime CreatedAt { get; set; } // Data salvarii jobului
}
