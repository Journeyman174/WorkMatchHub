using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface ISavedJobService
{
    Task<ServiceResponse<PagedResponse<SavedJobDTO>>> GetSavedJobs(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddSavedJob(SavedJobAddDTO savedJob, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteSavedJob(Guid jobOfferId, UserDTO requestingUser, CancellationToken cancellationToken = default);
}
