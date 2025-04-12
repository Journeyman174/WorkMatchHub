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
public class JobOfferController(IJobOfferService jobOfferService, IUserService userService) : AuthorizedController(userService)
{
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<JobOfferDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobOfferService.GetJobOffer(id, cancellationToken))
            : ErrorMessageResult<JobOfferDTO>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<JobOfferDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobOfferService.GetJobOffers(pagination, cancellationToken))
            : ErrorMessageResult<PagedResponse<JobOfferDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] JobOfferAddDTO jobOffer, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobOfferService.AddJobOffer(jobOffer, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Update([FromRoute] Guid id, [FromBody] JobOfferUpdateDTO jobOffer, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobOfferService.UpdateJobOffer(id, jobOffer, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await jobOfferService.DeleteJobOffer(id, currentUser.Result, cancellationToken))
            : ErrorMessageResult(currentUser.Error);
    }
}
