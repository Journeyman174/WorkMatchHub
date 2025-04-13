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

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class JobRequestService(IRepository<WebAppDatabaseContext> repository) : IJobRequestService
{
    public async Task<ServiceResponse<JobRequestDTO>> GetJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
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
        if (requestingUser.Role != UserRoleEnum.JobSeeker || !requestingUser.IsVerified)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var jobOffer = await repository.GetAsync(new JobOfferSpec(jobRequest.JobOfferId), cancellationToken);
        if (jobOffer == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobOfferNotFound);
        }

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
        var entity = await repository.GetAsync(new JobRequestSpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobRequestNotFound);
        }

        if (requestingUser.Role != UserRoleEnum.Admin && entity.UserId != requestingUser.Id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        await repository.DeleteAsync<JobRequest>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
