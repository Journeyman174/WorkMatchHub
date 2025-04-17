using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Gestioneaza operatiile pentru joburile salvate de utilizatori.
namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class SavedJobService(IRepository<WebAppDatabaseContext> repository) : ISavedJobService
{
    // Returneaza o lista paginata cu joburile salvate ale utilizatorului curent.
    public async Task<ServiceResponse<PagedResponse<SavedJobDTO>>> GetSavedJobs(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var spec = new SavedJobProjectionSpec(pagination.Search, requestingUser.Id);
        var result = await repository.PageAsync(pagination, spec, cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    // Adauga un job salvat pentru utilizatorul curent.
    // Identifica oferta de job dupa titlu si numele companiei.
    public async Task<ServiceResponse> AddSavedJob(SavedJobAddDTO savedJob, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca datele sunt valide
        if (savedJob == null || string.IsNullOrWhiteSpace(savedJob.JobTitle) || string.IsNullOrWhiteSpace(savedJob.CompanyName))
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobOfferData);
        }

        // Verifica daca userul este verificat
        if(!requestingUser.IsVerified)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Cauta oferta de job dupa titlu si companie
        var jobOffer = await repository.GetAsync(new JobOfferSpec(savedJob.JobTitle.Trim(), savedJob.CompanyName.Trim(), true), cancellationToken);
        if (jobOffer == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobOfferNotFound);
        }

        // Verifica daca jobul a fost deja salvat
        var existing = await repository.GetAsync(new SavedJobSpec(requestingUser.Id, jobOffer.Id), cancellationToken);
        if (existing != null)
        {
            return ServiceResponse.FromError(CommonErrors.JobAlreadySaved);
        }

        // Previne salvarea propriului job (recruiterul)
        if (jobOffer.UserId == requestingUser.Id)
        {
            return ServiceResponse.FromError(CommonErrors.CannotSaveOwnJob);
        }

        // Salveaza jobul
        await repository.AddAsync(new SavedJob
        {
            UserId = requestingUser.Id,
            JobOfferId = jobOffer.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    // Sterge un job salvat pentru utilizatorul curent.
    public async Task<ServiceResponse> DeleteSavedJob(Guid jobOfferId, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new SavedJobSpec(requestingUser.Id, jobOfferId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.SavedJobNotFound);
        }

        await repository.DeleteEntityAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    // Returneaza un job salvat specific dupa Id.
    public async Task<ServiceResponse<SavedJobDTO>> GetSavedJobById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new SavedJobProjectionSpec(id), cancellationToken);
        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<SavedJobDTO>(CommonErrors.SavedJobNotFound);
    }

    // Returneaza toate joburile salvate de un utilizator fara paginare.
    public async Task<ServiceResponse<List<SavedJobDTO>>> GetAllSavedJobsForUser(UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await repository.ListAsync(new SavedJobProjectionSpec(null, requestingUser.Id), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    // Cauta in joburile salvate dupa titlul jobului.
    public async Task<ServiceResponse<List<SavedJobDTO>>> SearchSavedJobs(string search, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return await GetAllSavedJobsForUser(requestingUser, cancellationToken);
        }

        var result = await repository.ListAsync(new SavedJobProjectionSpec(search, requestingUser.Id), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    // Verifica daca un anumit job este salvat de utilizator.
    public async Task<bool> IsJobSaved(Guid jobOfferId, Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new SavedJobSpec(userId, jobOfferId), cancellationToken);
        return result != null;
    }
}