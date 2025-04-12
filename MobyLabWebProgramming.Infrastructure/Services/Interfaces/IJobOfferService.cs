using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IJobOfferService
{
    Task<ServiceResponse<JobOfferDTO>> GetJobOffer(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<JobOfferDTO>>> GetJobOffers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddJobOffer(JobOfferAddDTO jobOffer, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateJobOffer(Guid id, JobOfferUpdateDTO jobOffer, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteJobOffer(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}
