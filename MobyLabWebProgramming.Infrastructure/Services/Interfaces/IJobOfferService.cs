using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata defineste operatiile disponibile pentru gestionarea ofertelor de munca.
public interface IJobOfferService
{
    // Returneaza o oferta de munca pe baza ID-ului.
    Task<ServiceResponse<JobOfferDTO>> GetJobOffer(Guid id, CancellationToken cancellationToken = default);

    // Returneaza o lista paginata de oferte de munca.
    Task<ServiceResponse<PagedResponse<JobOfferDTO>>> GetJobOffers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    // Adauga o noua oferta de munca in sistem.
    Task<ServiceResponse> AddJobOffer(JobOfferAddDTO jobOffer, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Actualizeaza o oferta de munca existenta.
    Task<ServiceResponse> UpdateJobOffer(Guid id, JobOfferUpdateDTO jobOffer, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Sterge o oferta de munca existenta.
    Task<ServiceResponse> DeleteJobOffer(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}