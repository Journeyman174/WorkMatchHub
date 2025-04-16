namespace MobyLabWebProgramming.Core.Entities;
public class Company : BaseEntity
{
    // Numele companiei - camp obligatoriu
    public string Name { get; set; } = null!;

    // Descrierea companiei (poate fi null)
    public string? Description { get; set; }

    // Locatia companiei (poate fi null)
    public string? Location { get; set; }

    // Id-ul utilizatorului caruia ii apartine compania - cheie straina catre entitatea User
    public Guid UserId { get; set; }

    // Colectie de oferte de munca asociate companiei - relatie One-to-Many
    public ICollection<JobOffer> JobOffers { get; set; } = null!;

    // Referinta catre obiectul User asociat - proprietate de navigare
    public User User { get; set; } = null!;
}