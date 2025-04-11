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

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class CompanyService(IRepository<WebAppDatabaseContext> repository) : ICompanyService
{
    public async Task<ServiceResponse<CompanyDTO>> GetCompany(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new CompanyProjectionSpec(id), cancellationToken);
        return result != null ? ServiceResponse.ForSuccess(result) : ServiceResponse.FromError<CompanyDTO>(CommonErrors.CompanyNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<CompanyDTO>>> GetCompanies(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new CompanyProjectionSpec(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddCompany(CompanyAddDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Recruiter)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var existingCompany = await repository.GetAsync(new CompanySpec(company.Name), cancellationToken);
        if (existingCompany != null)
        {
            return ServiceResponse.FromError(CommonErrors.CompanyAlreadyExists);
        }

        await repository.AddAsync(new Company
        {
            Name = company.Name,
            Description = company.Description,
            Location = company.Location,
            UserId = requestingUser.Id
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateCompany(Guid id, CompanyUpdateDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Recruiter)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var entity = await repository.GetAsync(new CompanySpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.CompanyNotFound);
        }

        entity.Name = company.Name ?? entity.Name;
        entity.Description = company.Description ?? entity.Description;
        entity.Location = company.Location ?? entity.Location;

        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteCompany(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Recruiter)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        var entity = await repository.GetAsync(new CompanySpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.CompanyNotFound);
        }

        await repository.DeleteAsync<Company>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
