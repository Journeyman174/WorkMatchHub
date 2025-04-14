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

public class JobAssignmentService(IRepository<WebAppDatabaseContext> repository) : IJobAssignmentService
{
    public async Task<ServiceResponse<JobAssignmentDTO>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new JobAssignmentProjectionSpec(id), cancellationToken);
        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.JobAssignmentNotFound);
    }


    public async Task<ServiceResponse<PagedResponse<JobAssignmentDTO>>> GetPage(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new JobAssignmentProjectionSpec(), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<JobAssignmentDTO>> AddJobAssignment(JobAssignmentAddDTO jobAssignment, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser.Role is not (UserRoleEnum.Admin or UserRoleEnum.Recruiter))
        {
            return ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.Forbidden);
        }

        var existingAssignment = await repository.GetAsync(new JobAssignmentSpec(jobAssignment.JobRequestId, true), cancellationToken);
        if (existingAssignment != null)
        {
            return ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.JobAssignmentAlreadyExists);
        }

        var newAssignment = new JobAssignment
        {
            Id = Guid.NewGuid(),
            JobRequestId = jobAssignment.JobRequestId,
            JobOfferId = jobAssignment.JobOfferId,
            AssignedAt = DateTime.UtcNow,
            UserId = requestingUser.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(newAssignment, cancellationToken);

        var result = await repository.GetAsync(new JobAssignmentProjectionSpec(newAssignment.Id), cancellationToken);
        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.JobAssignmentNotFound);
    }

    public async Task<ServiceResponse> DeleteJobAssignment(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser.Role is not (UserRoleEnum.Admin or UserRoleEnum.Recruiter))
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var assignment = await repository.GetAsync(new JobAssignmentSpec(id), cancellationToken);
        if (assignment == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobAssignmentNotFound);
        }

        await repository.DeleteEntityAsync(assignment, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
