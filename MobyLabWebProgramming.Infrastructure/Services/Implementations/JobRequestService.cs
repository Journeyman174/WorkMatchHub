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

// Gestioneaza operatiile legate de cererile de job.
namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class JobRequestService(IRepository<WebAppDatabaseContext> repository) : IJobRequestService
{
    public async Task<ServiceResponse<JobRequestDTO>> GetJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca Id-ul este valid
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
        // Verifica rolul si statusul utilizatorului
        if (requestingUser.Role != UserRoleEnum.JobSeeker || !requestingUser.IsVerified)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Valideaza datele introduse
        if (jobRequest == null ||
            string.IsNullOrWhiteSpace(jobRequest.CoverLetter) ||
            string.IsNullOrWhiteSpace(jobRequest.JobTitle) ||
            string.IsNullOrWhiteSpace(jobRequest.CompanyName))
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobRequestData);
        }

        // Cauta oferta de job pe baza titlului si a numelui companiei
        var jobOffer = await repository.GetAsync(
            new JobOfferSpec(jobRequest.JobTitle.Trim(), jobRequest.CompanyName.Trim(), true),
            cancellationToken
        );


        if (jobOffer == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobOfferNotFound);
        }

        // Verifica daca utilizatorul a aplicat deja
        var existing = await repository.GetAsync(new JobRequestSpec(requestingUser.Id, jobOffer.Id), cancellationToken);
        if (existing != null)
        {
            return ServiceResponse.FromError(CommonErrors.JobRequestAlreadyExists);
        }

        // Salveaza cererea
        await repository.AddAsync(new JobRequest
        {
            JobOfferId = jobOffer.Id,
            UserId = requestingUser.Id,
            CoverLetter = jobRequest.CoverLetter
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateJobRequest(JobRequestUpdateDTO updateDTO, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Doar JobSeeker verificat poate face update
        if (requestingUser.Role != UserRoleEnum.JobSeeker || !requestingUser.IsVerified)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Validare input
        if (updateDTO == null || string.IsNullOrWhiteSpace(updateDTO.CoverLetter) || string.IsNullOrWhiteSpace(updateDTO.JobTitle) || string.IsNullOrWhiteSpace(updateDTO.CompanyName))
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobRequestData);
        }

        // Cauta jobul dupa titlu si companie
        var jobOffer = await repository.GetAsync(new JobOfferSpec(updateDTO.JobTitle.Trim(), updateDTO.CompanyName.Trim(), true), cancellationToken);

        if (jobOffer == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobOfferNotFound);
        }

        // Cauta cererea de job existenta a userului pentru acest job
        var jobRequest = await repository.GetAsync(new JobRequestSpec(requestingUser.Id, jobOffer.Id), cancellationToken);
        if (jobRequest == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobRequestNotFound);
        }

        // Verifica daca jobul nu s-a schimbat intre timp
        if (jobRequest.JobOffer.Title != updateDTO.JobTitle.Trim() || jobRequest.JobOffer.Company.Name != updateDTO.CompanyName.Trim())
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobRequestData);
        }

        // Update
        jobRequest.CoverLetter = updateDTO.CoverLetter;
        await repository.UpdateAsync(jobRequest, cancellationToken);

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