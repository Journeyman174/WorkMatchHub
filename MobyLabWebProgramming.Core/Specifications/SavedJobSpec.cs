using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class SavedJobSpec : Specification<SavedJob>
{
    // Filtreaza dupa ID-ul utilizatorului si al ofertei de job pentru a verifica daca acel job este deja salvat.
    public SavedJobSpec(Guid userId, Guid jobOfferId)
    {
        Query.Where(sj => sj.UserId == userId && sj.JobOfferId == jobOfferId);
    }

    // Returneaza toate joburile salvate de un utilizator.
    public SavedJobSpec(Guid userId)
    {
        Query.Where(sj => sj.UserId == userId);
    }

    // Cauta in joburile salvate de un utilizator dupa titlul jobului.
    public SavedJobSpec(Guid userId, string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchExpr = $"%{search.Trim().Replace(" ", "%")}%";
            Query.Where(sj => sj.UserId == userId && EF.Functions.ILike(sj.JobOffer.Title, searchExpr));
        }
    }

    // Returneaza joburile salvate de un utilizator, ordonate descrescator dupa data salvarii.
    public SavedJobSpec(Guid userId, bool orderByCreatedAt)
    {
        Query.Where(sj => sj.UserId == userId);

        if (orderByCreatedAt)
            Query.OrderByDescending(sj => sj.CreatedAt);
    }
}
