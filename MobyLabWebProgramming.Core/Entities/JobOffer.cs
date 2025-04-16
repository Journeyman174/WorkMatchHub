namespace MobyLabWebProgramming.Core.Entities;
public class JobOffer : BaseEntity
{
    // Titlul postului - camp obligatoriu
    public string Title { get; set; } = null!;

    // Descrierea postului - camp obligatoriu
    public string Description { get; set; } = null!;

    // Salariul - camp obligatoriu
    public decimal Salary { get; set; }

    // Id-ul companiei care ofera jobul - cheie straina catre Company
    public Guid CompanyId { get; set; }

    // Referinta catre compania asociata - proprietate de navigare
    public Company Company { get; set; } = null!;

    // Id-ul utilizatorului care a creat oferta (recruiter) - cheie straina
    public Guid UserId { get; set; }

    // Referinta catre recruiterul care a creat oferta - proprietate de navigare
    public User User { get; set; } = null!;

    // Relatie One-to-Many cu entitatea JobAssignment - asocieri de joburi cu candidati selectati
    public ICollection<JobAssignment> JobAssignments { get; set; } = null!;

    // Relatie One-to-Many cu entitatea JobRequest - cereri de aplicare la acest job
    public ICollection<JobRequest> JobRequests { get; set; } = null!;

    // Relatie Many-to-Many cu utilizatori care au salvat acest job
    public ICollection<SavedJob> SavedByUsers { get; set; } = null!;
}