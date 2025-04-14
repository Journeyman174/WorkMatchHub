using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Clasa de specificatie pentru operatii care nu necesita proiectii (folosita pentru operatii simple pe entitati Company).
public sealed class CompanySpec : Specification<Company>
{
    // Specificatie pentru a gasi o companie dupa ID-ul acesteia.
    public CompanySpec(Guid id) => Query.Where(e => e.Id == id);

    // Specificatie pentru a gasi o companie dupa nume (folosit pentru validarea existentei).
    public CompanySpec(string name) => Query.Where(e => e.Name == name);
}
