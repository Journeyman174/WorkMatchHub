using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// Controller pentru gestionarea atribuirilor de job. 
/// Ofera functionalitati pentru adaugarea, stergerea si vizualizarea atribuirilor de catre recruiter sau admin.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class JobAssignmentController(IJobAssignmentService jobAssignmentService, IUserService userService)
    : AuthorizedController(userService)
{
    /// <summary>
    /// Returneaza o atribuire de job specifica dupa Id-ul atribuirii.
    /// </summary>
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<JobAssignmentDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return FromServiceResponse(await jobAssignmentService.GetById(id, cancellationToken));
    }

    /// <summary>
    /// Creeaza o atribuire de job pentru un utilizator de catre un recruiter sau admin.
    /// Se bazeaza pe titlul jobului si email-ul job seeker-ului.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] JobAssignmentAddDTO assignment, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobAssignmentService.AddJobAssignment(assignment, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Actualizeaza data atribuirii unei alocari de job. Doar adminul sau recruiterul poate modifica.
    /// </summary>
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Update([FromRoute] Guid id, [FromBody] JobAssignmentUpdateDTO updateDTO, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobAssignmentService.UpdateJobAssignment(id, updateDTO, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Sterge o atribuire de job dupa Id. Este permisa doar pentru admin sau recruiter-ul care a facut atribuirea.
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobAssignmentService.DeleteJobAssignment(id, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Returneaza o lista paginata de atribuiri de job filtrata in functie de utilizatorul curent si rolul acestuia.
    /// </summary>
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