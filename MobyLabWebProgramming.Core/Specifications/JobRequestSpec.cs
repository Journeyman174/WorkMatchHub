using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Specificatie pentru a obtine un JobRequest dupa Id-ul sau.
public sealed class JobRequestSpec : Specification<JobRequest>
{
    public JobRequestSpec(Guid id)
    {
        // Cauta dupa Id.
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

    // Specificatie pentru a obtine o cerere dupa titlul jobului si emailul utilizatorului.
    public JobRequestSpec(string jobTitle, string userEmail)
    {
        Query
            .Include(jr => jr.JobOffer)
            .Include(jr => jr.User)
            .Where(jr => jr.JobOffer.Title == jobTitle && jr.User.Email == userEmail);
    }

}