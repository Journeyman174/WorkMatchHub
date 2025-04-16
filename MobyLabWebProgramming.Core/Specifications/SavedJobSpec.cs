using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class SavedJobSpec : Specification<SavedJob>
{
    // Filtreaza dupa Id-ul utilizatorului si al ofertei de job pentru a verifica daca acel job este deja salvat.
    public SavedJobSpec(Guid userId, Guid jobOfferId)
    {
        Query.Where(sj => sj.UserId == userId && sj.JobOfferId == jobOfferId);
    }

}