using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class JobRequestProjectionSpec : Specification<JobRequest, JobRequestDTO>
{
    public JobRequestProjectionSpec(bool orderByCreatedAt = false)
    {
        Query.Include(jr => jr.JobOffer)
             .ThenInclude(j => j.Company);

        Query.Include(jr => jr.User);

        Query.Select(jr => new JobRequestDTO
        {
            Id = jr.Id,
            JobOffer = new JobOfferDTO
            {
                Id = jr.JobOffer.Id,
                Title = jr.JobOffer.Title,
                Description = jr.JobOffer.Description,
                Salary = jr.JobOffer.Salary,
                CreatedAt = jr.JobOffer.CreatedAt
            },

            User = new UserDTO
            {
                Id = jr.User.Id,
                Name = jr.User.Name,
                Email = jr.User.Email,
                Role = jr.User.Role,
                FullName = jr.User.FullName,
                IsVerified = jr.User.IsVerified
            },
            CoverLetter = jr.CoverLetter,
            CreatedAt = jr.CreatedAt
        });

        if (orderByCreatedAt)
        {
            Query.OrderByDescending(jr => jr.CreatedAt);
        }
    }

    public JobRequestProjectionSpec(Guid id) : this()
    {
        Query.Where(jr => jr.Id == id);
    }

    public JobRequestProjectionSpec(string? search, Guid userId, UserRoleEnum role) : this(true)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(jr => EF.Functions.ILike(jr.CoverLetter, searchExpr));
        }

        if (role == UserRoleEnum.JobSeeker)
        {
            Query.Where(jr => jr.User.Id == userId);
        }
        else if (role == UserRoleEnum.Recruiter)
        {
            Query.Where(jr => jr.JobOffer.User.Id == userId);
        }
    }
}
