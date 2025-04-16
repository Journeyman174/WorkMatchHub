namespace MobyLabWebProgramming.Core.Entities;
public class JobRequest : BaseEntity
{
    // Id-ul ofertei de job la care utilizatorul aplica - cheie straina catre JobOffer
    // Relatie Many-to-One: mai multe cereri pot fi asociate aceleiasi oferte
    public Guid JobOfferId { get; set; }

    // Id-ul utilizatorului care a trimis cererea - cheie straina catre User
    // Relatie Many-to-One: un utilizator poate avea mai multe cereri
    public Guid UserId { get; set; }

    // Scrisoarea de intentie - camp obligatoriu completat de candidat
    public string CoverLetter { get; set; } = null!;

    // Proprietate de navigare catre oferta de job - permite acces la detalii despre job
    public JobOffer JobOffer { get; set; } = null!;

    // Proprietate de navigare catre utilizatorul care a aplicat
    public User User { get; set; } = null!;

    // Proprietate de navigare catre o posibila atribuire a jobului
    // Relatie One-to-One: o cerere poate avea o atribuire
    public JobAssignment JobAssignment { get; set; } = null!;
}