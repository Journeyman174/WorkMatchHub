using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class CompanyProjectionSpec : Specification<Company, CompanyDTO>
{
    public CompanyProjectionSpec(bool orderByCreatedAt = false)
    {
        Query.Select(e => new CompanyDTO
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description
        });

        if (orderByCreatedAt)
        {
            Query.OrderByDescending(e => e.CreatedAt);
        }
    }

    public CompanyProjectionSpec(Guid id) : this() => Query.Where(e => e.Id == id);

    public CompanyProjectionSpec(string? search) : this(true)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
    }
}
