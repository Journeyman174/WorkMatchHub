using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class SavedJobProjectionSpec : Specification<SavedJob, SavedJobDTO>
{
    public SavedJobProjectionSpec(string? search, Guid userId)
    {
        Query.Where(sj => sj.UserId == userId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(sj => EF.Functions.ILike(sj.JobOffer.Title, searchExpr));
        }

        Query.Select(sj => new SavedJobDTO
        {
            JobOffer = new JobOfferDTO
            {
                Id = sj.JobOffer.Id,
                Title = sj.JobOffer.Title,
                Description = sj.JobOffer.Description,
                
            },
            CreatedAt = sj.CreatedAt
        })
        .OrderByDescending(sj => sj.CreatedAt);
    }
}
