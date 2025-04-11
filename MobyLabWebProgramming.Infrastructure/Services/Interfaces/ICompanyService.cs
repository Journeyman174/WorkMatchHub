using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface ICompanyService
{
    Task<ServiceResponse<CompanyDTO>> GetCompany(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<CompanyDTO>>> GetCompanies(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddCompany(CompanyAddDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateCompany(Guid id, CompanyUpdateDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteCompany(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}
