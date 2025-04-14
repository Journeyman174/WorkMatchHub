using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class JobAssignmentProjectionSpec : Specification<JobAssignment, JobAssignmentDTO>
{
    public JobAssignmentProjectionSpec(Guid id)
    {
        Query.Where(e => e.Id == id);

        Query
            .Include(e => e.JobRequest)
                .ThenInclude(jr => jr.User)
            .Include(e => e.JobRequest)
                .ThenInclude(jr => jr.JobOffer)
                    .ThenInclude(jo => jo.Company)
                        .ThenInclude(c => c.User)
            .Include(e => e.JobOffer)
                .ThenInclude(jo => jo.User)
            .Include(e => e.JobOffer)
                .ThenInclude(jo => jo.Company)
                    .ThenInclude(c => c.User);

        Query.Select(e => new JobAssignmentDTO
        {
            Id = e.Id,
            AssignedAt = e.AssignedAt,
            CreatedAt = e.CreatedAt,
            User = new UserDTO
            {
                Id = e.JobRequest.User.Id,
                Name = e.JobRequest.User.Name,
                Email = e.JobRequest.User.Email,
                Role = e.JobRequest.User.Role,
                FullName = e.JobRequest.User.FullName,
                IsVerified = e.JobRequest.User.IsVerified,
                CompanyName = e.JobRequest.User.Company != null ? e.JobRequest.User.Company.Name : ""
            },
            JobRequest = new JobRequestDTO
            {
                Id = e.JobRequest.Id,
                CoverLetter = e.JobRequest.CoverLetter,
                CreatedAt = e.JobRequest.CreatedAt,
                User = new UserDTO
                {
                    Id = e.JobRequest.User.Id,
                    Name = e.JobRequest.User.Name,
                    Email = e.JobRequest.User.Email,
                    Role = e.JobRequest.User.Role,
                    FullName = e.JobRequest.User.FullName,
                    IsVerified = e.JobRequest.User.IsVerified,
                    CompanyName = e.JobRequest.User.Company != null ? e.JobRequest.User.Company.Name : ""
                },
                JobOffer = new JobOfferDTO
                {
                    Id = e.JobRequest.JobOffer.Id,
                    Title = e.JobRequest.JobOffer.Title,
                    Description = e.JobRequest.JobOffer.Description,
                    Salary = e.JobRequest.JobOffer.Salary,
                    CreatedAt = e.JobRequest.JobOffer.CreatedAt,
                    Company = new CompanyDTO
                    {
                        Id = e.JobRequest.JobOffer.Company.Id,
                        Name = e.JobRequest.JobOffer.Company.Name,
                        Description = e.JobRequest.JobOffer.Company.Description,
                        Location = e.JobRequest.JobOffer.Company.Location,
                        CreatedAt = e.JobRequest.JobOffer.Company.CreatedAt,
                        User = new UserDTO
                        {
                            Id = e.JobRequest.JobOffer.Company.User.Id,
                            Name = e.JobRequest.JobOffer.Company.User.Name,
                            Email = e.JobRequest.JobOffer.Company.User.Email,
                            Role = e.JobRequest.JobOffer.Company.User.Role,
                            FullName = e.JobRequest.JobOffer.Company.User.FullName,
                            IsVerified = e.JobRequest.JobOffer.Company.User.IsVerified,
                            CompanyName = e.JobRequest.JobOffer.Company.User.Company != null ? e.JobRequest.JobOffer.Company.User.Company!.Name : ""
                        }
                    },
                    Recruiter = new UserDTO
                    {
                        Id = e.JobRequest.JobOffer.User.Id,
                        Name = e.JobRequest.JobOffer.User.Name,
                        Email = e.JobRequest.JobOffer.User.Email,
                        Role = e.JobRequest.JobOffer.User.Role,
                        FullName = e.JobRequest.JobOffer.User.FullName,
                        IsVerified = e.JobRequest.JobOffer.User.IsVerified,
                        CompanyName = e.JobRequest.JobOffer.User.Company != null ? e.JobRequest.JobOffer.User.Company.Name : ""
                    }
                }
            },
            JobOffer = new JobOfferDTO
            {
                Id = e.JobOffer.Id,
                Title = e.JobOffer.Title,
                Description = e.JobOffer.Description,
                Salary = e.JobOffer.Salary,
                CreatedAt = e.JobOffer.CreatedAt,
                Company = new CompanyDTO
                {
                    Id = e.JobOffer.Company.Id,
                    Name = e.JobOffer.Company.Name,
                    Description = e.JobOffer.Company.Description,
                    Location = e.JobOffer.Company.Location,
                    CreatedAt = e.JobOffer.Company.CreatedAt,
                    User = new UserDTO
                    {
                        Id = e.JobOffer.Company.User.Id,
                        Name = e.JobOffer.Company.User.Name,
                        Email = e.JobOffer.Company.User.Email,
                        Role = e.JobOffer.Company.User.Role,
                        FullName = e.JobOffer.Company.User.FullName,
                        IsVerified = e.JobOffer.Company.User.IsVerified,
                        CompanyName = e.JobOffer.Company.User.Company != null ? e.JobOffer.Company.User.Company.Name : ""
                    }
                },
                Recruiter = new UserDTO
                {
                    Id = e.JobOffer.User.Id,
                    Name = e.JobOffer.User.Name,
                    Email = e.JobOffer.User.Email,
                    Role = e.JobOffer.User.Role,
                    FullName = e.JobOffer.User.FullName,
                    IsVerified = e.JobOffer.User.IsVerified,
                    CompanyName = e.JobOffer.User.Company != null ? e.JobOffer.User.Company.Name : ""
                }
            }
        });
    }

    public JobAssignmentProjectionSpec()
    {
        Query
            .Include(e => e.JobRequest)
                .ThenInclude(jr => jr.User)
            .Include(e => e.JobRequest)
                .ThenInclude(jr => jr.JobOffer)
                    .ThenInclude(jo => jo.Company)
                        .ThenInclude(c => c.User)
            .Include(e => e.JobOffer)
                .ThenInclude(jo => jo.User)
            .Include(e => e.JobOffer)
                .ThenInclude(jo => jo.Company)
                    .ThenInclude(c => c.User);

        Query.Select(e => new JobAssignmentDTO
        {
            Id = e.Id,
            AssignedAt = e.AssignedAt,
            CreatedAt = e.CreatedAt,
            User = new UserDTO
            {
                Id = e.JobRequest.User.Id,
                Name = e.JobRequest.User.Name,
                Email = e.JobRequest.User.Email,
                Role = e.JobRequest.User.Role,
                FullName = e.JobRequest.User.FullName,
                IsVerified = e.JobRequest.User.IsVerified,
                CompanyName = e.JobRequest.User.Company != null ? e.JobRequest.User.Company.Name : ""
            },
            JobRequest = new JobRequestDTO
            {
                Id = e.JobRequest.Id,
                CoverLetter = e.JobRequest.CoverLetter,
                CreatedAt = e.JobRequest.CreatedAt,
                User = new UserDTO
                {
                    Id = e.JobRequest.User.Id,
                    Name = e.JobRequest.User.Name,
                    Email = e.JobRequest.User.Email,
                    Role = e.JobRequest.User.Role,
                    FullName = e.JobRequest.User.FullName,
                    IsVerified = e.JobRequest.User.IsVerified,
                    CompanyName = e.JobRequest.User.Company != null ? e.JobRequest.User.Company.Name : ""
                },
                JobOffer = new JobOfferDTO
                {
                    Id = e.JobRequest.JobOffer.Id,
                    Title = e.JobRequest.JobOffer.Title,
                    Description = e.JobRequest.JobOffer.Description,
                    Salary = e.JobRequest.JobOffer.Salary,
                    CreatedAt = e.JobRequest.JobOffer.CreatedAt,
                    Company = new CompanyDTO
                    {
                        Id = e.JobRequest.JobOffer.Company.Id,
                        Name = e.JobRequest.JobOffer.Company.Name,
                        Description = e.JobRequest.JobOffer.Company.Description,
                        Location = e.JobRequest.JobOffer.Company.Location,
                        CreatedAt = e.JobRequest.JobOffer.Company.CreatedAt,
                        User = new UserDTO
                        {
                            Id = e.JobRequest.JobOffer.Company.User.Id,
                            Name = e.JobRequest.JobOffer.Company.User.Name,
                            Email = e.JobRequest.JobOffer.Company.User.Email,
                            Role = e.JobRequest.JobOffer.Company.User.Role,
                            FullName = e.JobRequest.JobOffer.Company.User.FullName,
                            IsVerified = e.JobRequest.JobOffer.Company.User.IsVerified,
                            CompanyName = e.JobRequest.JobOffer.Company.User.Company != null ? e.JobRequest.JobOffer.Company.User.Company!.Name : ""
                        }
                    },
                    Recruiter = new UserDTO
                    {
                        Id = e.JobRequest.JobOffer.User.Id,
                        Name = e.JobRequest.JobOffer.User.Name,
                        Email = e.JobRequest.JobOffer.User.Email,
                        Role = e.JobRequest.JobOffer.User.Role,
                        FullName = e.JobRequest.JobOffer.User.FullName,
                        IsVerified = e.JobRequest.JobOffer.User.IsVerified,
                        CompanyName = e.JobRequest.JobOffer.User.Company != null ? e.JobRequest.JobOffer.User.Company.Name : ""
                    }
                }
            },
            JobOffer = new JobOfferDTO
            {
                Id = e.JobOffer.Id,
                Title = e.JobOffer.Title,
                Description = e.JobOffer.Description,
                Salary = e.JobOffer.Salary,
                CreatedAt = e.JobOffer.CreatedAt,
                Company = new CompanyDTO
                {
                    Id = e.JobOffer.Company.Id,
                    Name = e.JobOffer.Company.Name,
                    Description = e.JobOffer.Company.Description,
                    Location = e.JobOffer.Company.Location,
                    CreatedAt = e.JobOffer.Company.CreatedAt,
                    User = new UserDTO
                    {
                        Id = e.JobOffer.Company.User.Id,
                        Name = e.JobOffer.Company.User.Name,
                        Email = e.JobOffer.Company.User.Email,
                        Role = e.JobOffer.Company.User.Role,
                        FullName = e.JobOffer.Company.User.FullName,
                        IsVerified = e.JobOffer.Company.User.IsVerified,
                        CompanyName = e.JobOffer.Company.User.Company != null ? e.JobOffer.Company.User.Company.Name : ""
                    }
                },
                Recruiter = new UserDTO
                {
                    Id = e.JobOffer.User.Id,
                    Name = e.JobOffer.User.Name,
                    Email = e.JobOffer.User.Email,
                    Role = e.JobOffer.User.Role,
                    FullName = e.JobOffer.User.FullName,
                    IsVerified = e.JobOffer.User.IsVerified,
                    CompanyName = e.JobOffer.User.Company != null ? e.JobOffer.User.Company.Name : ""
                }
            }
        });
    }
}
