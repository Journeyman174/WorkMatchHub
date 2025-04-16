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

// Gestioneaza operatiile pentru entitatea JobOffer.
public class JobOfferService(IRepository<WebAppDatabaseContext> repository) : IJobOfferService
{
    // Returneaza o oferta de job dupa Id
    public async Task<ServiceResponse<JobOfferDTO>> GetJobOffer(Guid id, CancellationToken cancellationToken = default)
    {
        // Verifica daca Id-ul este valid
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

        // Cauta toate companiile asociate utilizatorului
        var companies = await repository.ListAsync(new CompanySpec(requestingUser.Id, isByUser: true), cancellationToken);

        // Verifica daca este asociat cu o singura companie
        if (companies.Count != 1)
        {
            return ServiceResponse.FromError(CommonErrors.MultipleCompaniesFound);
        }

        var company = companies.First();

        // Creeaza oferta de job cu datele furnizate si compania identificata
        await repository.AddAsync(new JobOffer
        {
            Title = jobOffer.Title,
            Description = jobOffer.Description,
            Salary = jobOffer.Salary,
            CompanyId = company.Id,
            UserId = requestingUser.Id
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }


    // Actualizeaza o oferta de job existenta.
    public async Task<ServiceResponse> UpdateJobOffer(Guid id, JobOfferUpdateDTO jobOffer, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca Id-ul si datele sunt valide
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
        // Verifica daca Id-ul este valid
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