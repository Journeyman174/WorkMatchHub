using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IJobAssignmentService
{
    Task<ServiceResponse<JobAssignmentDTO>> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<ServiceResponse<PagedResponse<JobAssignmentDTO>>> GetPage(
        PaginationSearchQueryParams pagination,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );

    Task<ServiceResponse<JobAssignmentDTO>> AddJobAssignment(
        JobAssignmentAddDTO jobAssignment,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );

    Task<ServiceResponse> DeleteJobAssignment(
        Guid id,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );


}
