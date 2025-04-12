using System.Net;
using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class SavedJobService(IRepository<WebAppDatabaseContext> repository) : ISavedJobService
{
    public async Task<ServiceResponse<PagedResponse<SavedJobDTO>>> GetSavedJobs(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var spec = new SavedJobProjectionSpec(pagination.Search, requestingUser.Id);
        var result = await repository.PageAsync(pagination, spec, cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddSavedJob(SavedJobAddDTO savedJob, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetAsync(new SavedJobSpec(requestingUser.Id, savedJob.JobOffer.Id), cancellationToken);

        if (existing != null)
        {
            return ServiceResponse.FromError(CommonErrors.JobAlreadySaved);
        }

        await repository.AddAsync(new SavedJob
        {
            UserId = requestingUser.Id,
            JobOfferId = savedJob.JobOffer.Id
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteSavedJob(Guid jobOfferId, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new SavedJobSpec(requestingUser.Id, jobOfferId), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.SavedJobNotFound);
        }

        await repository.DeleteEntityAsync(entity, cancellationToken); // folosește noua metodă

        return ServiceResponse.ForSuccess();
    }

}
