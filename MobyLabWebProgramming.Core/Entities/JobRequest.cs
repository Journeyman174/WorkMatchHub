using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Entities;

public class JobRequest : BaseEntity
{
    public Guid JobOfferId { get; set; }
    public Guid UserId { get; set; } 

    public string CoverLetter { get; set; } = null!;

    // Relatii
    public JobOffer JobOffer { get; set; } = null!;
    public User User { get; set; } = null!; // Legam userul prin UserId
    public JobAssignment JobAssignment { get; set; } = null!;
}
