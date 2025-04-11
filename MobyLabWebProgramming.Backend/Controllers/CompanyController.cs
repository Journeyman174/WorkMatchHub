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
public class CompanyController(ICompanyService companyService, IUserService userService) : AuthorizedController(userService)
{
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<CompanyDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await companyService.GetCompany(id, cancellationToken))
            : ErrorMessageResult<CompanyDTO>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<CompanyDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await companyService.GetCompanies(pagination, cancellationToken))
            : ErrorMessageResult<PagedResponse<CompanyDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] CompanyAddDTO company, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await companyService.AddCompany(company, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Update([FromRoute] Guid id, [FromBody] CompanyUpdateDTO company, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await companyService.UpdateCompany(id, company, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

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
