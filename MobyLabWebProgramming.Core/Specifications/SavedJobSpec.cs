using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class SavedJobSpec : Specification<SavedJob>
{
    public SavedJobSpec(Guid userId, Guid jobOfferId)
    {
        Query.Where(sj => sj.UserId == userId && sj.JobOfferId == jobOfferId);
    }
}
