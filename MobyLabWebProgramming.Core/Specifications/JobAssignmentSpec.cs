using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class JobAssignmentSpec : Specification<JobAssignment>
{
    public JobAssignmentSpec(Guid id)
    {
        Query.Where(e => e.Id == id)
             .Include(e => e.JobRequest)
             .Include(e => e.JobOffer);
    }

    public JobAssignmentSpec(Guid jobRequestId, bool byRequest)
    {
        if (byRequest)
        {
            Query.Where(e => e.JobRequestId == jobRequestId)
                 .Include(e => e.JobRequest)
                 .Include(e => e.JobOffer);
        }
    }
}
