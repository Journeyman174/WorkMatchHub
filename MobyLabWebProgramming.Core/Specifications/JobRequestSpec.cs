using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Specificatie pentru a obtine un JobRequest dupa Id-ul sau.
public sealed class JobRequestSpec : Specification<JobRequest>
{
    public JobRequestSpec(Guid id)
    {
        // Cauta dupa Id si include relatiile necesare pentru a preveni campuri null.
        Query
            .Where(jr => jr.Id == id)
            .Include(jr => jr.User)
            .Include(jr => jr.JobOffer);
    }

    // Specificatie pentru a verifica daca un utilizator a aplicat la un anumit job.
    public JobRequestSpec(Guid userId, Guid jobOfferId)
    {
        Query
            .Where(jr => jr.User.Id == userId && jr.JobOffer.Id == jobOfferId)
            .Include(jr => jr.User)
            .Include(jr => jr.JobOffer);
    }
}