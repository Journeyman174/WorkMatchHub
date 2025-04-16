using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata pentru serviciul care gestioneaza asignarile de joburi.
public interface IJobAssignmentService
{
    // Returneaza detaliile unei asignari pe baza Id-ului.
    Task<ServiceResponse<JobAssignmentDTO>> GetById(Guid id, CancellationToken cancellationToken = default);

    // Returneaza toate asignarile paginate pentru utilizatorul curent.
    Task<ServiceResponse<PagedResponse<JobAssignmentDTO>>> GetPage(
        PaginationSearchQueryParams pagination,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );

    // Creeaza o atribuire de job pentru un job seeker identificat prin email si titlul jobului. Doar adminii si recruiterii pot crea.
    Task<ServiceResponse> AddJobAssignment(JobAssignmentAddDTO jobAssignment, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Actualizeaza o alocare de job. Doar adminul sau recruiterul care a facut alocarea poate modifica data atribuirii.
    Task<ServiceResponse> UpdateJobAssignment(Guid id, JobAssignmentUpdateDTO updateDTO, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Sterge o asignare existenta. Doar adminul sau recruiterul care a facut alocarea poate sterge.
    Task<ServiceResponse> DeleteJobAssignment(
        Guid id,
        UserDTO requestingUser,
        CancellationToken cancellationToken = default
    );
}