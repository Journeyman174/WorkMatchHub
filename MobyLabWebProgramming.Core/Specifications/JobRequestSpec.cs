using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class JobRequestSpec : Specification<JobRequest>
{
    public JobRequestSpec(Guid id)
    {
        Query.Where(jr => jr.Id == id);
    }

    public JobRequestSpec(Guid userId, Guid jobOfferId)
    {
        Query.Where(jr => jr.User.Id == userId && jr.JobOffer.Id == jobOfferId);
    }
}
