using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class JobOfferProjectionSpec : Specification<JobOffer, JobOfferDTO>
{
    public JobOfferProjectionSpec(bool orderByCreatedAt = false)
    {
        Query.Include(e => e.Company);
        Query.Include(e => e.User);

        Query.Select(e => new JobOfferDTO
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Salary = e.Salary,
            Company = new CompanyDTO
            {
                Id = e.Company.Id,
                Name = e.Company.Name,
                Description = e.Company.Description,
                Location = e.Company.Location
            },
            Recruiter = new UserDTO
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Email = e.User.Email
            },
            CreatedAt = e.CreatedAt
        });

        if (orderByCreatedAt)
        {
            Query.OrderByDescending(e => e.CreatedAt);
        }
    }

    public JobOfferProjectionSpec(Guid id) : this()
    {
        Query.Where(e => e.Id == id);
    }

    public JobOfferProjectionSpec(string? search) : this(true)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(e => EF.Functions.ILike(e.Title, searchExpr));
        }
    }
}
