using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Specificatii pentru filtrarea entitatii JobOffer
public sealed class JobOfferSpec : Specification<JobOffer>
{
    // Constructor pentru cautarea unui job offer dupa ID-ul acestuia
    public JobOfferSpec(Guid id)
    {
        Query
            .Where(offer => offer.Id == id)
            .Include(offer => offer.Company) // Include compania asociata
                .ThenInclude(company => company.User) // Include user-ul companiei
            .Include(offer => offer.User); // Include recrutorul
    }

    // Constructor pentru cautarea unui job offer dupa titlu
    public JobOfferSpec(string title)
    {
        Query.Where(offer => offer.Title == title);
    }

    // Constructor pentru a extrage job-urile unui anumit recrutor
    public JobOfferSpec(Guid recruiterId, bool isByRecruiter)
    {
        Query
            .Where(offer => offer.UserId == recruiterId)
            .Include(offer => offer.Company)
                .ThenInclude(company => company.User)
            .Include(offer => offer.User);
    }
}