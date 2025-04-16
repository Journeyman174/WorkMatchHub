using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata defineste operatiile disponibile pentru serviciul SavedJob.
public interface ISavedJobService
{
    // Returneaza o lista paginata cu joburile salvate ale utilizatorului curent.
    Task<ServiceResponse<PagedResponse<SavedJobDTO>>> GetSavedJobs(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Adauga un job salvat pentru utilizatorul curent.
    Task<ServiceResponse> AddSavedJob(SavedJobAddDTO savedJob, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Sterge un job salvat pentru utilizatorul curent.
    Task<ServiceResponse> DeleteSavedJob(Guid jobOfferId, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Returneaza un job salvat specific dupa Id.
    Task<ServiceResponse<SavedJobDTO>> GetSavedJobById(Guid id, CancellationToken cancellationToken = default);

    // Returneaza toate joburile salvate de un utilizator fara paginare.
    Task<ServiceResponse<List<SavedJobDTO>>> GetAllSavedJobsForUser(UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Cauta in joburile salvate dupa titlul jobului.
    Task<ServiceResponse<List<SavedJobDTO>>> SearchSavedJobs(string search, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Verifica daca un anumit job este salvat de utilizator.
    Task<bool> IsJobSaved(Guid jobOfferId, Guid userId, CancellationToken cancellationToken = default);
}