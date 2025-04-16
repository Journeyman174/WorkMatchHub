namespace MobyLabWebProgramming.Core.Entities;
public class JobAssignment : BaseEntity
{
    // Id-ul cererii de job acceptata - cheie straina catre JobRequest
    // Relatie One-to-One: fiecare atribuire este legata de o singura cerere acceptata
    public Guid JobRequestId { get; set; }

    // Id-ul ofertei de job care a fost atribuita - cheie straina catre JobOffer
    // Relatie Many-to-One: mai multe atribuiri pot fi legate de aceeasi oferta
    public Guid JobOfferId { get; set; }

    // Data la care jobul a fost atribuit utilizatorului
    public DateTime AssignedAt { get; set; }

    // Id-ul utilizatorului caruia i s-a atribuit jobul - cheie straina
    // Acelasi cu UserId-ul din JobRequest
    public Guid UserId { get; set; }

    // Proprietate de navigare catre cererea de job acceptata
    public JobRequest JobRequest { get; set; } = null!;

    // Proprietate de navigare catre oferta de job atribuita
    public JobOffer JobOffer { get; set; } = null!;
}