using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

// Specificatie pentru proiectarea SavedJob in DTO si filtrare dupa utilizator, titlul jobului sau id-ul SavedJob
namespace MobyLabWebProgramming.Core.Specifications;

public sealed class SavedJobProjectionSpec : Specification<SavedJob, SavedJobDTO>
{
    // Constructor pentru cautare dupa text si filtrare dupa utilizator
    public SavedJobProjectionSpec(string? search, Guid userId)
    {
        Query
            .Where(sj => sj.UserId == userId)
            .Include(sj => sj.JobOffer)
                .ThenInclude(jo => jo.Company)
                    .ThenInclude(c => c.User)
            .Include(sj => sj.JobOffer.User)
            .Include(sj => sj.User);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(sj => EF.Functions.ILike(sj.JobOffer.Title, searchExpr));
        }

        Query.Select(sj => new SavedJobDTO
        {
            Id = sj.Id,
            CreatedAt = sj.CreatedAt,
            JobOffer = new JobOfferDTO
            {
                Id = sj.JobOffer.Id,
                Title = sj.JobOffer.Title,
                Description = sj.JobOffer.Description,
                Salary = sj.JobOffer.Salary,
                CreatedAt = sj.JobOffer.CreatedAt,
                Company = new CompanyDTO
                {
                    Id = sj.JobOffer.Company.Id,
                    Name = sj.JobOffer.Company.Name,
                    Description = sj.JobOffer.Company.Description,
                    Location = sj.JobOffer.Company.Location,
                    CreatedAt = sj.JobOffer.Company.CreatedAt,
                    User = new UserDTO
                    {
                        Id = sj.JobOffer.Company.User.Id,
                        Name = sj.JobOffer.Company.User.Name,
                        Email = sj.JobOffer.Company.User.Email,
                        Role = sj.JobOffer.Company.User.Role,
                        FullName = sj.JobOffer.Company.User.FullName,
                        IsVerified = sj.JobOffer.Company.User.IsVerified,
                        CompanyName = sj.JobOffer.Company.User.Company != null ? sj.JobOffer.Company.User.Company.Name : ""
                    }
                },
                Recruiter = new UserDTO
                {
                    Id = sj.JobOffer.User.Id,
                    Name = sj.JobOffer.User.Name,
                    Email = sj.JobOffer.User.Email,
                    Role = sj.JobOffer.User.Role,
                    FullName = sj.JobOffer.User.FullName,
                    IsVerified = sj.JobOffer.User.IsVerified,
                    CompanyName = sj.JobOffer.User.Company != null ? sj.JobOffer.User.Company.Name : ""
                }
            },
            User = new UserDTO
            {
                Id = sj.User.Id,
                Name = sj.User.Name,
                Email = sj.User.Email,
                Role = sj.User.Role,
                FullName = sj.User.FullName,
                IsVerified = sj.User.IsVerified,
                CompanyName = sj.User.Company != null ? sj.User.Company.Name : ""
            }
        })
        .OrderByDescending(sj => sj.CreatedAt);
    }

    // Constructor pentru filtrare doar dupa userId
    public SavedJobProjectionSpec(Guid userId)
    {
        Query
            .Where(sj => sj.UserId == userId)
            .Include(sj => sj.JobOffer)
                .ThenInclude(jo => jo.Company)
                    .ThenInclude(c => c.User)
            .Include(sj => sj.JobOffer.User)
            .Include(sj => sj.User);

        Query.Select(sj => new SavedJobDTO
        {
            Id = sj.Id,
            CreatedAt = sj.CreatedAt,
            JobOffer = new JobOfferDTO
            {
                Id = sj.JobOffer.Id,
                Title = sj.JobOffer.Title,
                Description = sj.JobOffer.Description,
                Salary = sj.JobOffer.Salary,
                CreatedAt = sj.JobOffer.CreatedAt,
                Company = new CompanyDTO
                {
                    Id = sj.JobOffer.Company.Id,
                    Name = sj.JobOffer.Company.Name,
                    Description = sj.JobOffer.Company.Description,
                    Location = sj.JobOffer.Company.Location,
                    CreatedAt = sj.JobOffer.Company.CreatedAt,
                    User = new UserDTO
                    {
                        Id = sj.JobOffer.Company.User.Id,
                        Name = sj.JobOffer.Company.User.Name,
                        Email = sj.JobOffer.Company.User.Email,
                        Role = sj.JobOffer.Company.User.Role,
                        FullName = sj.JobOffer.Company.User.FullName,
                        IsVerified = sj.JobOffer.Company.User.IsVerified,
                        CompanyName = sj.JobOffer.Company.User.Company != null ? sj.JobOffer.Company.User.Company.Name : ""
                    }
                },
                Recruiter = new UserDTO
                {
                    Id = sj.JobOffer.User.Id,
                    Name = sj.JobOffer.User.Name,
                    Email = sj.JobOffer.User.Email,
                    Role = sj.JobOffer.User.Role,
                    FullName = sj.JobOffer.User.FullName,
                    IsVerified = sj.JobOffer.User.IsVerified,
                    CompanyName = sj.JobOffer.User.Company != null ? sj.JobOffer.User.Company.Name : ""
                }
            },
            User = new UserDTO
            {
                Id = sj.User.Id,
                Name = sj.User.Name,
                Email = sj.User.Email,
                Role = sj.User.Role,
                FullName = sj.User.FullName,
                IsVerified = sj.User.IsVerified,
                CompanyName = sj.User.Company != null ? sj.User.Company.Name : ""
            }
        });
    }

    // Constructor pentru cautare dupa id-ul jobului salvat
    public SavedJobProjectionSpec(Guid id, bool isById)
    {
        Query
            .Where(sj => sj.Id == id)
            .Include(sj => sj.JobOffer)
                .ThenInclude(jo => jo.Company)
                    .ThenInclude(c => c.User)
            .Include(sj => sj.JobOffer.User)
            .Include(sj => sj.User);

        Query.Select(sj => new SavedJobDTO
        {
            Id = sj.Id,
            CreatedAt = sj.CreatedAt,
            JobOffer = new JobOfferDTO
            {
                Id = sj.JobOffer.Id,
                Title = sj.JobOffer.Title,
                Description = sj.JobOffer.Description,
                Salary = sj.JobOffer.Salary,
                CreatedAt = sj.JobOffer.CreatedAt,
                Company = new CompanyDTO
                {
                    Id = sj.JobOffer.Company.Id,
                    Name = sj.JobOffer.Company.Name,
                    Description = sj.JobOffer.Company.Description,
                    Location = sj.JobOffer.Company.Location,
                    CreatedAt = sj.JobOffer.Company.CreatedAt,
                    User = new UserDTO
                    {
                        Id = sj.JobOffer.Company.User.Id,
                        Name = sj.JobOffer.Company.User.Name,
                        Email = sj.JobOffer.Company.User.Email,
                        Role = sj.JobOffer.Company.User.Role,
                        FullName = sj.JobOffer.Company.User.FullName,
                        IsVerified = sj.JobOffer.Company.User.IsVerified,
                        CompanyName = sj.JobOffer.Company.User.Company != null ? sj.JobOffer.Company.User.Company.Name : ""
                    }
                },
                Recruiter = new UserDTO
                {
                    Id = sj.JobOffer.User.Id,
                    Name = sj.JobOffer.User.Name,
                    Email = sj.JobOffer.User.Email,
                    Role = sj.JobOffer.User.Role,
                    FullName = sj.JobOffer.User.FullName,
                    IsVerified = sj.JobOffer.User.IsVerified,
                    CompanyName = sj.JobOffer.User.Company != null ? sj.JobOffer.User.Company.Name : ""
                }
            },
            User = new UserDTO
            {
                Id = sj.User.Id,
                Name = sj.User.Name,
                Email = sj.User.Email,
                Role = sj.User.Role,
                FullName = sj.User.FullName,
                IsVerified = sj.User.IsVerified,
                CompanyName = sj.User.Company != null ? sj.User.Company.Name : ""
            }
        });
    }
}