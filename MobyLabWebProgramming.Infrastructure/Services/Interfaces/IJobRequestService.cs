using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IJobRequestService
{
    Task<ServiceResponse<JobRequestDTO>> GetJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<JobRequestDTO>>> GetJobRequests(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddJobRequest(JobRequestAddDTO jobRequest, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}
