using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Clasa folosita pentru a crea specificatii de proiectie ale entitatii Company in DTO-uri pentru a fi folosite in serviciile backend.
public sealed class CompanyProjectionSpec : Specification<Company, CompanyDTO>
{
    // Constructorul default permite ordonarea companiilor dupa data crearii si include utilizatorul asociat companiei.
    public CompanyProjectionSpec(bool orderByCreatedAt = false)
    {
        Query.Include(e => e.User); // Include utilizatorul asociat companiei.

        // Selecteaza campurile ce vor fi mapate in DTO.
        Query.Select(e => new CompanyDTO
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Location = e.Location,
            User = new UserDTO
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Email = e.User.Email,
                Role = e.User.Role,
                FullName = e.User.FullName ?? "",
                IsVerified = e.User.IsVerified,
                CompanyName = e.User.Company != null ? e.User.Company.Name : ""
            },
            CreatedAt = e.CreatedAt
        });

        if (orderByCreatedAt)
        {
            Query.OrderByDescending(e => e.CreatedAt); // Ordine descrescatoare dupa data crearii daca este specificat.
        }
    }

    // Constructor pentru a selecta o companie dupa Id-ul acesteia.
    public CompanyProjectionSpec(Guid id) : this() => Query.Where(e => e.Id == id);

    // Constructor pentru cautare dupa numele companiei.
    public CompanyProjectionSpec(string? search) : this(true)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
    }
}