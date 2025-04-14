using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// Controller responsabil pentru gestionarea joburilor salvate de utilizatori.
/// Permite adaugarea, stergerea, listarea, cautarea si verificarea existentei joburilor salvate.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class SavedJobsController(ISavedJobService savedJobService, IUserService userService) : AuthorizedController(userService)
{
    /// <summary>
    /// Returneaza o lista paginata de joburi salvate de utilizatorul curent.
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<SavedJobDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.GetSavedJobs(pagination, currentUser.Result, cancellationToken))
            : ErrorMessageResult<PagedResponse<SavedJobDTO>>(currentUser.Error);
    }

    /// <summary>
    /// Adauga un job in lista de joburi salvate de utilizatorul curent.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] SavedJobAddDTO savedJob, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.AddSavedJob(savedJob, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Sterge un job salvat pe baza Id-ului ofertei de job.
    /// </summary>
    [Authorize]
    [HttpDelete("{jobOfferId:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid jobOfferId, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.DeleteSavedJob(jobOfferId, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Returneaza toate joburile salvate de utilizatorul curent, fara paginare.
    /// </summary>
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<RequestResponse<List<SavedJobDTO>>>> GetAll(CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.GetAllSavedJobsForUser(currentUser.Result, cancellationToken))
            : ErrorMessageResult<List<SavedJobDTO>>(currentUser.Error);
    }

    /// <summary>
    /// Cauta in lista de joburi salvate dupa titlul jobului.
    /// </summary>
    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<RequestResponse<List<SavedJobDTO>>>> Search([FromQuery] string search, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await savedJobService.SearchSavedJobs(search, currentUser.Result, cancellationToken))
            : ErrorMessageResult<List<SavedJobDTO>>(currentUser.Error);
    }

    /// <summary>
    /// Verifica daca un anumit job este salvat de utilizatorul curent.
    /// </summary>
    [Authorize]
    [HttpGet("is-saved/{jobOfferId:guid}")]
    public async Task<ActionResult<bool>> IsSaved([FromRoute] Guid jobOfferId, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? Ok(await savedJobService.IsJobSaved(jobOfferId, currentUser.Result.Id, cancellationToken))
            : Unauthorized();
    }

    /// <summary>
    /// Returneaza un job salvat specific dupa Id-ul salvarii (nu Id-ul ofertei).
    /// </summary>
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