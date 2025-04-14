using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

// Serviciul gestioneaza operatiile pentru entitatile de tip JobOffer.
public class JobOfferService(IRepository<WebAppDatabaseContext> repository) : IJobOfferService
{
    // Returneaza o oferta de job dupa ID.
    public async Task<ServiceResponse<JobOfferDTO>> GetJobOffer(Guid id, CancellationToken cancellationToken = default)
    {
        // Verifica daca ID-ul este valid
        if (id == Guid.Empty)
        {
            return ServiceResponse.FromError<JobOfferDTO>(CommonErrors.InvalidId);
        }

        var result = await repository.GetAsync(new JobOfferProjectionSpec(id), cancellationToken);
        return result != null ? ServiceResponse.ForSuccess(result) : ServiceResponse.FromError<JobOfferDTO>(CommonErrors.JobOfferNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<JobOfferDTO>>> GetJobOffers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        // Returneaza lista paginata de oferte
        var result = await repository.PageAsync(pagination, new JobOfferProjectionSpec(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    // Adauga o oferta de job noua.
    public async Task<ServiceResponse> AddJobOffer(JobOfferAddDTO jobOffer, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca datele sunt valide
        if (jobOffer == null || string.IsNullOrWhiteSpace(jobOffer.Title) || jobOffer.Salary <= 0)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobOfferData);
        }

        // Verifica daca userul are dreptul de a crea o oferta
        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Recruiter)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Verifica daca compania exista
        var company = await repository.GetAsync(new CompanySpec(jobOffer.CompanyId), cancellationToken);
        if (company == null)
        {
            return ServiceResponse.FromError(CommonErrors.CompanyNotFound);
        }

        // Verifica daca recruiterul are acces la aceasta companie
        if (requestingUser.Role == UserRoleEnum.Recruiter && company.UserId != requestingUser.Id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        await repository.AddAsync(new JobOffer
        {
            Title = jobOffer.Title,
            Description = jobOffer.Description,
            Salary = jobOffer.Salary,
            CompanyId = jobOffer.CompanyId,
            UserId = requestingUser.Id
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    // Actualizeaza o oferta de job existenta.
    public async Task<ServiceResponse> UpdateJobOffer(Guid id, JobOfferUpdateDTO jobOffer, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca ID-ul si datele sunt valide
        if (id == Guid.Empty || jobOffer == null)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobOfferData);
        }

        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Recruiter)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var entity = await repository.GetAsync(new JobOfferSpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobOfferNotFound);
        }

        if (requestingUser.Role == UserRoleEnum.Recruiter && entity.UserId != requestingUser.Id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Actualizeaza doar campurile furnizate
        entity.Title = jobOffer.Title ?? entity.Title;
        entity.Description = jobOffer.Description ?? entity.Description;
        entity.Salary = jobOffer.Salary ?? entity.Salary;

        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    // Sterge o oferta de job.
    public async Task<ServiceResponse> DeleteJobOffer(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca ID-ul este valid
        if (id == Guid.Empty)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidId);
        }

        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Recruiter)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var entity = await repository.GetAsync(new JobOfferSpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobOfferNotFound);
        }

        if (requestingUser.Role == UserRoleEnum.Recruiter && entity.UserId != requestingUser.Id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        await repository.DeleteAsync<JobOffer>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}