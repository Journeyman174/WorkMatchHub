using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata defineste operatiile disponibile pentru serviciul SavedJob.
public interface ISavedJobService
{
    Task<ServiceResponse<PagedResponse<SavedJobDTO>>> GetSavedJobs(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddSavedJob(SavedJobAddDTO savedJob, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteSavedJob(Guid jobOfferId, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse<SavedJobDTO>> GetSavedJobById(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<List<SavedJobDTO>>> GetAllSavedJobsForUser(UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse<List<SavedJobDTO>>> SearchSavedJobs(string search, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<bool> IsJobSaved(Guid jobOfferId, Guid userId, CancellationToken cancellationToken = default);
}