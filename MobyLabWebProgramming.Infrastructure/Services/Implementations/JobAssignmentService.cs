using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Gestioneaza operatiile de asignare a unui job pentru un job seeker.
namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class JobAssignmentService(IRepository<WebAppDatabaseContext> repository, IMailService mailService) : IJobAssignmentService
{
    // Returneaza detalii despre o asignare in functie de Id-ul acesteia.
    public async Task<ServiceResponse<JobAssignmentDTO>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new JobAssignmentProjectionSpec(id), cancellationToken);
        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.JobAssignmentNotFound);
    }

    // Returneaza o lista paginata cu toate asignarile de joburi.
    public async Task<ServiceResponse<PagedResponse<JobAssignmentDTO>>> GetPage(PaginationSearchQueryParams pagination, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new JobAssignmentProjectionSpec(), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    // Creeaza o atribuire noua pentru un job seeker, doar daca utilizatorul este Admin sau Recruiter.
    public async Task<ServiceResponse> AddJobAssignment(JobAssignmentAddDTO jobAssignment, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Valideaza datele primite
        if (jobAssignment == null || string.IsNullOrWhiteSpace(jobAssignment.JobTitle) || string.IsNullOrWhiteSpace(jobAssignment.JobSeekerEmail))
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobAssignmentData);
        }

        // Doar Adminii si Recruiterii au dreptul sa creeze atribuirile
        if (requestingUser.Role is not (UserRoleEnum.Admin or UserRoleEnum.Recruiter))
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Cauta cererea de job dupa titlu si email
        var jobRequest = await repository.GetAsync(new JobRequestSpec(jobAssignment.JobTitle.Trim(), jobAssignment.JobSeekerEmail.Trim()), cancellationToken);

        if (jobRequest == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobRequestNotFound);
        }

        // Verifica daca deja exista o atribuire pentru aceasta cerere
        var existingAssignment = await repository.GetAsync(new JobAssignmentSpec(jobRequest.Id, true), cancellationToken);
        if (existingAssignment != null)
        {
            return ServiceResponse.FromError(CommonErrors.JobAssignmentAlreadyExists);
        }

        // Creeaza o noua atribuire
        var newAssignment = new JobAssignment
        {
            Id = Guid.NewGuid(),
            JobRequestId = jobRequest.Id,
            JobOfferId = jobRequest.JobOfferId,
            AssignedAt = DateTime.UtcNow,
            UserId = jobRequest.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(newAssignment, cancellationToken);

        // Trimite email de notificare catre utilizator
        var result = await repository.GetAsync(new JobAssignmentProjectionSpec(newAssignment.Id), cancellationToken);
        if (result != null)
        {
            var jobSeeker = result.User;
            var jobTitle = result.JobOffer?.Title ?? "a job";
            var jobDescription = result.JobOffer?.Description ?? "No job description provided.";

            if (!string.IsNullOrEmpty(jobSeeker?.Email))
            {
                await mailService.SendMail(
                    jobSeeker.Email,
                    "You've been accepted!",
                    MailTemplates.JobAssignmentAcceptedTemplate(
                        jobSeeker.FullName ?? jobSeeker.Name,
                        jobTitle,
                        jobDescription
                    ),
                    true,
                    "WorkMatchHub",
                    cancellationToken
                );
            }

            return ServiceResponse.ForSuccess();
        }

        return ServiceResponse.FromError(CommonErrors.JobAssignmentNotFound);
    }

    public async Task<ServiceResponse> UpdateJobAssignment(Guid id, JobAssignmentUpdateDTO updateDTO, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Doar Adminii si Recruiterii pot actualiza alocarile
        if (requestingUser.Role is not (UserRoleEnum.Admin or UserRoleEnum.Recruiter))
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Validare date primite
        if (updateDTO == null || updateDTO.AssignedAt == null)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidJobAssignmentData);
        }

        // Cauta alocarea dupa Id
        var assignment = await repository.GetAsync(new JobAssignmentSpec(id), cancellationToken);
        if (assignment == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobAssignmentNotFound);
        }

        // Daca este recruiter, trebuie sa fie si cel care a postat jobul
        if (requestingUser.Role == UserRoleEnum.Recruiter && assignment.JobOffer.UserId != requestingUser.Id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Actualizeaza data atribuirii
        assignment.AssignedAt = updateDTO.AssignedAt.Value;
        assignment.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(assignment, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    // Sterge o asignare de job, daca utilizatorul este Admin sau Recruiter.
    public async Task<ServiceResponse> DeleteJobAssignment(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser.Role is not (UserRoleEnum.Admin or UserRoleEnum.Recruiter))
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var assignment = await repository.GetAsync(new JobAssignmentSpec(id), cancellationToken);
        if (assignment == null)
        {
            return ServiceResponse.FromError(CommonErrors.JobAssignmentNotFound);
        }

        await repository.DeleteEntityAsync(assignment, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}