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
public class SavedJobsController(ISavedJobService savedJobService, IUserService userService) : AuthorizedController(userService)
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<SavedJobDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.GetSavedJobs(pagination, currentUser.Result, cancellationToken))
            : ErrorMessageResult<PagedResponse<SavedJobDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] SavedJobAddDTO savedJob, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.AddSavedJob(savedJob, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{jobOfferId:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid jobOfferId, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.DeleteSavedJob(jobOfferId, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }
}
