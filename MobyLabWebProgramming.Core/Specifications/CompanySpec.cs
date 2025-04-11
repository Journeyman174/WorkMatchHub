using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class CompanySpec : Specification<Company>
{
    public CompanySpec(Guid id) => Query.Where(e => e.Id == id);
    public CompanySpec(string name) => Query.Where(e => e.Name == name);
}
