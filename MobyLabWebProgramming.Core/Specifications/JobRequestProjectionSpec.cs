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
        Query
            .Include(jr => jr.JobOffer)
                .ThenInclude(jo => jo.Company)
                    .ThenInclude(c => c.User)
            .Include(jr => jr.JobOffer.User)
                .ThenInclude(u => u.Company)
            .Include(jr => jr.User);

        Query.Select(jr => new JobRequestDTO
        {
            Id = jr.Id,
            CoverLetter = jr.CoverLetter,
            CreatedAt = jr.CreatedAt,

            JobOffer = new JobOfferDTO
            {
                Id = jr.JobOffer.Id,
                Title = jr.JobOffer.Title,
                Description = jr.JobOffer.Description,
                Salary = jr.JobOffer.Salary,
                CreatedAt = jr.JobOffer.CreatedAt,

                Company = new CompanyDTO
                {
                    Id = jr.JobOffer.Company.Id,
                    Name = jr.JobOffer.Company.Name,
                    Description = jr.JobOffer.Company.Description,
                    Location = jr.JobOffer.Company.Location,
                    CreatedAt = jr.JobOffer.Company.CreatedAt,
                    User = new UserDTO
                    {
                        Id = jr.JobOffer.Company.User.Id,
                        Name = jr.JobOffer.Company.User.Name,
                        Email = jr.JobOffer.Company.User.Email,
                        Role = jr.JobOffer.Company.User.Role,
                        FullName = jr.JobOffer.Company.User.FullName,
                        IsVerified = jr.JobOffer.Company.User.IsVerified,
                        CompanyName = jr.JobOffer.Company.User.Company!.Name
                    }
                },

                Recruiter = new UserDTO
                {
                    Id = jr.JobOffer.User.Id,
                    Name = jr.JobOffer.User.Name,
                    Email = jr.JobOffer.User.Email,
                    Role = jr.JobOffer.User.Role,
                    FullName = jr.JobOffer.User.FullName,
                    IsVerified = jr.JobOffer.User.IsVerified,
                    CompanyName = jr.JobOffer.User.Company!.Name
                }
            },

            User = new UserDTO
            {
                Id = jr.User.Id,
                Name = jr.User.Name,
                Email = jr.User.Email,
                Role = jr.User.Role,
                FullName = jr.User.FullName,
                IsVerified = jr.User.IsVerified,
                CompanyName = jr.User.Company != null ? jr.User.Company.Name : ""
            }
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
