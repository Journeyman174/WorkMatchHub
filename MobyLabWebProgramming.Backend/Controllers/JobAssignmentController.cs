using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class JobAssignmentController(IJobAssignmentService jobAssignmentService, IUserService userService)
    : AuthorizedController(userService)
{
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<JobAssignmentDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return FromServiceResponse(await jobAssignmentService.GetById(id, cancellationToken));
    }


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse<JobAssignmentDTO>>> Add([FromBody] JobAssignmentAddDTO assignment, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobAssignmentService.AddJobAssignment(assignment, currentUser.Result, cancellationToken))
            : ErrorMessageResult<JobAssignmentDTO>(currentUser.Error);
    }


    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobAssignmentService.DeleteJobAssignment(id, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<JobAssignmentDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobAssignmentService.GetPage(pagination, currentUser.Result, cancellationToken))
            : ErrorMessageResult<PagedResponse<JobAssignmentDTO>>(currentUser.Error);
    }

}
