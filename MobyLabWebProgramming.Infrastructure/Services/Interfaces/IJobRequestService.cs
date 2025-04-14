using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata defineste operatiile pentru gestionarea cererilor de job.
public interface IJobRequestService
{
    // Returneaza o cerere de job dupa ID, doar daca utilizatorul are permisiune.
    Task<ServiceResponse<JobRequestDTO>> GetJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Returneaza o lista paginata de cereri de job filtrata dupa utilizator.
    Task<ServiceResponse<PagedResponse<JobRequestDTO>>> GetJobRequests(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Adauga o cerere de job pentru un utilizator JobSeeker verificat.
    Task<ServiceResponse> AddJobRequest(JobRequestAddDTO jobRequest, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Sterge o cerere de job daca utilizatorul este admin sau autorul cererii.
    Task<ServiceResponse> DeleteJobRequest(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}