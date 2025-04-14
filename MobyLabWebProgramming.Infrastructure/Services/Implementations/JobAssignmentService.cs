using System.Net;
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

// Serviciul gestioneaza operatiile de asignare a unui job pentru un job seeker.
namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class JobAssignmentService(IRepository<WebAppDatabaseContext> repository, IMailService mailService) : IJobAssignmentService
{
    // Returneaza detalii despre o asignare in functie de ID-ul acesteia.
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

    // Creeaza o asignare noua pentru un job seeker, doar daca utilizatorul este Admin sau Recruiter.
    public async Task<ServiceResponse<JobAssignmentDTO>> AddJobAssignment(JobAssignmentAddDTO jobAssignment, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Verifica daca datele sunt valide
        if (jobAssignment == null)
        {
            return ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.InvalidJobAssignmentData);
        }

        // Doar Adminii si Recruiterii pot face asignari
        if (requestingUser.Role is not (UserRoleEnum.Admin or UserRoleEnum.Recruiter))
        {
            return ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.Forbidden);
        }

        // Verifica daca exista deja o asignare pentru cererea respectiva
        var existingAssignment = await repository.GetAsync(new JobAssignmentSpec(jobAssignment.JobRequestId, true), cancellationToken);
        if (existingAssignment != null)
        {
            return ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.JobAssignmentAlreadyExists);
        }

        // Creeaza o noua asignare
        var newAssignment = new JobAssignment
        {
            Id = Guid.NewGuid(),
            JobRequestId = jobAssignment.JobRequestId,
            JobOfferId = jobAssignment.JobOfferId,
            AssignedAt = DateTime.UtcNow,
            UserId = requestingUser.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(newAssignment, cancellationToken);

        // Trimite mail de notificare catre job seeker
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

            return ServiceResponse.ForSuccess(result);
        }

        return ServiceResponse.FromError<JobAssignmentDTO>(CommonErrors.JobAssignmentNotFound);
    }

    // Sterge o asignare de job, doar daca utilizatorul este Admin sau Recruiter.
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