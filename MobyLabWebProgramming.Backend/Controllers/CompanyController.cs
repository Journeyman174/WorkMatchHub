using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// Controllerul gestioneaza operatiile CRUD pentru companii.
/// Accesul este permis doar utilizatorilor autentificati.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class CompanyController(ICompanyService companyService, IUserService userService) : AuthorizedController(userService)
{
    /// <summary>
    /// Returneaza o companie pe baza Id-ului.
    /// </summary>
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<CompanyDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await companyService.GetCompany(id, cancellationToken))
            : ErrorMessageResult<CompanyDTO>(currentUser.Error);
    }

    /// <summary>
    /// Returneaza o lista paginata de companii cu optiune de cautare.
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<CompanyDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await companyService.GetCompanies(pagination, cancellationToken))
            : ErrorMessageResult<PagedResponse<CompanyDTO>>(currentUser.Error);
    }

    /// <summary>
    /// Adauga o noua companie. Doar recrutorii si adminii pot adauga companii.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] CompanyAddDTO company, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await companyService.AddCompany(company, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Actualizeaza informatiile unei companii existente.
    /// </summary>
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Update([FromRoute] Guid id, [FromBody] CompanyUpdateDTO company, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await companyService.UpdateCompany(id, company, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Sterge o companie pe baza Id-ului. Doar recrutorii care detin compania sau adminii pot face acest lucru.
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await companyService.DeleteCompany(id, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }
}