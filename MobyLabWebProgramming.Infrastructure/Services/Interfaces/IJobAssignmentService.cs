using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata pentru serviciul care gestioneaza asignarile de joburi.
public interface IJobAssignmentService
{
    // Returneaza detaliile unei asignari pe baza ID-ului.
    Task<ServiceResponse<JobAssignmentDTO>> GetById(Guid id, CancellationToken cancellationToken = default);

    // Returneaza toate asignarile paginate pentru utilizatorul curent.
    Task<ServiceResponse<PagedResponse<JobAssignmentDTO>>> GetPage(
        PaginationSearchQueryParams pagination,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );

    // Adauga o asignare noua a unui job pentru un job seeker.
    Task<ServiceResponse<JobAssignmentDTO>> AddJobAssignment(
        JobAssignmentAddDTO jobAssignment,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );

    // Sterge o asignare existenta.
    Task<ServiceResponse> DeleteJobAssignment(
        Guid id,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );
}
