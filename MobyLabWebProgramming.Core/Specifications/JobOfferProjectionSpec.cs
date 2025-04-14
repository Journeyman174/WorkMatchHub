using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class JobOfferProjectionSpec : Specification<JobOffer, JobOfferDTO>
{
    // Constructor ce permite ordonarea optionala dupa data crearii
    public JobOfferProjectionSpec(bool orderByCreatedAt = false)
    {
        // Include entitatile legate pentru companie si recruiter
        Query.Include(e => e.Company)
             .ThenInclude(c => c.User); // Include utilizatorul asociat companiei
        Query.Include(e => e.User); // Include recruiterul

        // Selecteaza datele in JobOfferDTO si maparea inlantuita
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
                    Role = e.Company.User.Role,
                    IsVerified = e.Company.User.IsVerified,
                    FullName = e.Company.User.FullName,
                    CompanyName = e.Company.User.Company != null ? e.Company.User.Company.Name : ""
                },
                CreatedAt = e.Company.CreatedAt
            },
            Recruiter = new UserDTO
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Email = e.User.Email,
                Role = e.User.Role,
                IsVerified = e.User.IsVerified,
                FullName = e.User.FullName,
                CompanyName = e.User.Company != null ? e.User.Company.Name : ""
            },
            CreatedAt = e.CreatedAt
        });

        if (orderByCreatedAt)
        {
            Query.OrderByDescending(e => e.CreatedAt); // Ordonare descrescatoare dupa data crearii
        }
    }

    // Constructor pentru cautare dupa ID
    public JobOfferProjectionSpec(Guid id) : this() =>
        Query.Where(e => e.Id == id);

    // Constructor pentru cautare cu filtrare dupa titlu (search)
    public JobOfferProjectionSpec(string? search) : this(true)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(e => EF.Functions.ILike(e.Title, searchExpr)); // Cautare partiala si case-insensitive
        }
    }
}