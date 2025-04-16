using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Clasa de specificatie pentru operatii care nu necesita proiectii (operatii pe entitatea Company).
public sealed class CompanySpec : Specification<Company>
{
    // Specificatie pentru a gasi o companie dupa Id-ul acesteia.
    public CompanySpec(Guid id) => Query.Where(e => e.Id == id);

    // Specificatie pentru a gasi o companie dupa nume (pentru validarea existentei).
    public CompanySpec(string name) => Query.Where(e => e.Name == name);

    // Specificatie pentru a gasi compania detinuta de un anumit utilizator (recruiter).
    public CompanySpec(Guid userId, bool isByUser) => Query.Where(e => e.UserId == userId);
}