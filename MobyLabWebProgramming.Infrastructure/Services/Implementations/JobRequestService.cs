using System.Net;
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

// Serviciul gestioneaza operatiile legate de cererile de job.
namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class JobRequestService(IRepository<WebAppDatabaseContext> repository) : IJobRequestService
{
    public async Task<ServiceResponse<JobRequestDTO>> GetJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca ID-ul este valid
        if (id == Guid.Empty)
            return ServiceResponse.FromError<JobRequestDTO>(CommonErrors.InvalidId);

        var result = await repository.GetAsync(new JobRequestProjectionSpec(id), cancellationToken);
        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<JobRequestDTO>(CommonErrors.JobRequestNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<JobRequestDTO>>> GetJobRequests(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var spec = new JobRequestProjectionSpec(pagination.Search, requestingUser.Id, requestingUser.Role);
        var result = await repository.PageAsync(pagination, spec, cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddJobRequest(JobRequestAddDTO jobRequest, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca utilizatorul are dreptul de a face cererea
        if (requestingUser.Role != UserRoleEnum.JobSeeker || !requestingUser.IsVerified)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Verifica daca datele cererii sunt valide
        if (jobRequest == null || jobRequest.JobOfferId == Guid.Empty)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobRequestData);
        }

        // Verifica daca oferta de job exista
        var jobOffer = await repository.GetAsync(new JobOfferSpec(jobRequest.JobOfferId), cancellationToken);
        if (jobOffer == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobOfferNotFound);
        }

        // Verifica daca deja a aplicat la jobul respectiv
        var existing = await repository.GetAsync(new JobRequestSpec(requestingUser.Id, jobRequest.JobOfferId), cancellationToken);
        if (existing != null)
        {
            return ServiceResponse.FromError(CommonErrors.JobRequestAlreadyExists);
        }

        // Adauga cererea noua in baza de date
        await repository.AddAsync(new JobRequest
        {
            JobOfferId = jobRequest.JobOfferId,
            UserId = requestingUser.Id,
            CoverLetter = jobRequest.CoverLetter
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidId);
        }

        var entity = await repository.GetAsync(new JobRequestSpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobRequestNotFound);
        }

        // Verifica permisiunea de stergere
        if (requestingUser.Role != UserRoleEnum.Admin && entity.UserId != requestingUser.Id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        await repository.DeleteAsync<JobRequest>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}