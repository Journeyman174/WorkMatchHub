using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Specificatie pentru a obtine entitati JobAssignment din baza de date
public sealed class JobAssignmentSpec : Specification<JobAssignment>
{
    // Constructor pentru a obtine un JobAssignment dupa ID-ul sau
    public JobAssignmentSpec(Guid id)
    {
        Query.Where(e => e.Id == id)
             .Include(e => e.JobRequest) // Include relatia cu cererea de job
             .Include(e => e.JobOffer);  // Include relatia cu oferta de job
    }

    // Constructor pentru a obtine un JobAssignment dupa ID-ul unei cereri (JobRequest)
    public JobAssignmentSpec(Guid jobRequestId, bool byRequest)
    {
        if (byRequest)
        {
            Query.Where(e => e.JobRequestId == jobRequestId)
                 .Include(e => e.JobRequest) // Include relatia cu cererea de job
                 .Include(e => e.JobOffer);  // Include relatia cu oferta de job
        }
    }
}