using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class JobOfferProjectionSpec : Specification<JobOffer, JobOfferDTO>
{
    public JobOfferProjectionSpec(bool orderByCreatedAt = false)
    {
        Query.Include(e => e.Company)
             .ThenInclude(c => c.User); // Include utilizatorul asociat companiei
        Query.Include(e => e.User); // Include recruiterul

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
                Location = e.Company.Location,
                User = new UserDTO
                {
                    Id = e.Company.User.Id,
                    Name = e.Company.User.Name,
                    Email = e.Company.User.Email,
                    Role = e.Company.User.Role
                },
                CreatedAt = e.Company.CreatedAt
            },
            Recruiter = new UserDTO
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Email = e.User.Email,
                Role = e.User.Role
            },
            CreatedAt = e.CreatedAt
        });

        if (orderByCreatedAt)
        {
            Query.OrderByDescending(e => e.CreatedAt);
        }
    }

    public JobOfferProjectionSpec(Guid id) : this() =>
        Query.Where(e => e.Id == id);

    public JobOfferProjectionSpec(string? search) : this(true)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(e => EF.Functions.ILike(e.Title, searchExpr));
        }
    }
}
