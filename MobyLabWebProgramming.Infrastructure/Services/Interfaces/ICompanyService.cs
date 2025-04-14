using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

// Interfata defineste operatiile disponibile pentru gestionarea companiilor.
public interface ICompanyService
{
    // Returneaza o companie dupa ID.
    Task<ServiceResponse<CompanyDTO>> GetCompany(Guid id, CancellationToken cancellationToken = default);

    // Returneaza o lista paginata de companii.
    Task<ServiceResponse<PagedResponse<CompanyDTO>>> GetCompanies(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    // Adauga o companie noua in sistem.
    Task<ServiceResponse> AddCompany(CompanyAddDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Actualizeaza o companie existenta dupa ID.
    Task<ServiceResponse> UpdateCompany(Guid id, CompanyUpdateDTO company, UserDTO requestingUser, CancellationToken cancellationToken = default);

    // Sterge o companie dupa ID.
    Task<ServiceResponse> DeleteCompany(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}
