using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata pentru operatiile disponibile pentru gestionarea companiilor.
public interface ICompanyService
{
    // Returneaza o companie dupa Id.
    Task<ServiceResponse<CompanyDTO>> GetCompany(Guid id, CancellationToken cancellationToken = default);

    // Returneaza o lista paginata de companii.
    Task<ServiceResponse<PagedResponse<CompanyDTO>>> GetCompanies(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    // Adauga o companie noua.
    Task<ServiceResponse> AddCompany(CompanyAddDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Actualizeaza o companie existenta dupa Id.
    Task<ServiceResponse> UpdateCompany(Guid id, CompanyUpdateDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Sterge o companie dupa Id.
    Task<ServiceResponse> DeleteCompany(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}