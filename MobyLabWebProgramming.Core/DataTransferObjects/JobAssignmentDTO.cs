namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a transmite informatii despre o alocare de job.
/// Contine referinte catre cererea de job, oferta si utilizatorul implicat.
/// </summary>
public class JobAssignmentDTO
{
    public Guid Id { get; set; } // Id-ul alocarii
    public JobRequestDTO JobRequest { get; set; } = null!; // Cererea de job asociata
    public JobOfferDTO JobOffer { get; set; } = null!; // Oferta de job asociata
    public DateTime AssignedAt { get; set; } // Data alocarii efective
    public UserDTO User { get; set; } = null!; // Utilizatorul implicat (JobSeeker)
    public DateTime CreatedAt { get; set; } // Data crearii inregistrarii
}
