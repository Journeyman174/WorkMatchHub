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
    // Returneaza joburile salvate de utilizator (paginat).
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<SavedJobDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.GetSavedJobs(pagination, currentUser.Result, cancellationToken))
            : ErrorMessageResult<PagedResponse<SavedJobDTO>>(currentUser.Error);
    }

    // Adauga un job salvat pentru utilizatorul curent.
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] SavedJobAddDTO savedJob, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.AddSavedJob(savedJob, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    // Sterge un job salvat dupa ID-ul ofertei.
    [Authorize]
    [HttpDelete("{jobOfferId:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid jobOfferId, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.DeleteSavedJob(jobOfferId, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    // Returneaza toate joburile salvate de utilizatorul curent (fara paginare).
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<RequestResponse<List<SavedJobDTO>>>> GetAll(CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.GetAllSavedJobsForUser(currentUser.Result, cancellationToken))
            : ErrorMessageResult<List<SavedJobDTO>>(currentUser.Error);
    }

    // Cauta joburi salvate dupa titlu.
    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<RequestResponse<List<SavedJobDTO>>>> Search([FromQuery] string search, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.SearchSavedJobs(search, currentUser.Result, cancellationToken))
            : ErrorMessageResult<List<SavedJobDTO>>(currentUser.Error);
    }

    // Verifica daca un job este salvat de utilizatorul curent.
    [Authorize]
    [HttpGet("is-saved/{jobOfferId:guid}")]
    public async Task<ActionResult<bool>> IsSaved([FromRoute] Guid jobOfferId, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? Ok(await savedJobService.IsJobSaved(jobOfferId, currentUser.Result.Id, cancellationToken))
            : Unauthorized();
    }

    // Returneaza un job salvat specific dupa ID.
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<SavedJobDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.GetSavedJobById(id, cancellationToken))
            : ErrorMessageResult<SavedJobDTO>(currentUser.Error);
    }
}
