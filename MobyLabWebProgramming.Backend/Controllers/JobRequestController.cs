using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// Controllerul gestioneaza operatiile pentru cererile de job.
/// Cererile pot fi trimise de utilizatorii autentificati si verificati.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class JobRequestController(IJobRequestService jobRequestService, IUserService userService) : AuthorizedController(userService)
{
    /// <summary>
    /// Returneaza o cerere de job pe baza Id-ului.
    /// </summary>
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<JobRequestDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await jobRequestService.GetJobRequest(id, currentUser.Result, cancellationToken))
            : ErrorMessageResult<JobRequestDTO>(currentUser.Error);
    }

    /// <summary>
    /// Returneaza o lista paginata de cereri de job. Lista este filtrata in functie de utilizator si rolul acestuia.
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<JobRequestDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await jobRequestService.GetJobRequests(pagination, currentUser.Result, cancellationToken))
            : ErrorMessageResult<PagedResponse<JobRequestDTO>>(currentUser.Error);
    }

    /// <summary>
    /// Adauga o noua cerere de job. Doar utilizatorii de tip JobSeeker verificati pot aplica.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] JobRequestAddDTO jobRequest, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobRequestService.AddJobRequest(jobRequest, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Actualizeaza scrisoarea de intentie pentru o cerere de job.
    /// Identificarea se face prin titlul jobului si numele companiei.
    /// Doar utilizatorii de tip JobSeeker verificati pot face modificarea.
    /// </summary>
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] JobRequestUpdateDTO updateDTO, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobRequestService.UpdateJobRequest(updateDTO, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Sterge o cerere de job. Cererea poate fi stearsa de JobSeeker-ul care a facut-o sau de un Admin.
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await jobRequestService.DeleteJobRequest(id, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }
}