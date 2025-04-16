using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

// Specificatii pentru filtrarea entitatii JobOffer
public sealed class JobOfferSpec : Specification<JobOffer>
{
    // Constructor pentru cautarea unui job offer dupa Id-ul acestuia
    public JobOfferSpec(Guid id)
    {
        Query
            .Where(offer => offer.Id == id)
            .Include(offer => offer.Company) // Compania asociata
                .ThenInclude(company => company.User) // User-ul companiei
            .Include(offer => offer.User); // Recrutorul
    }

    // Constructor pentru cautarea unui job offer dupa titlu si numele companiei
    public JobOfferSpec(string title, string companyName, bool byTitleAndCompany)
    {
        if (byTitleAndCompany)
        {
            Query
                .Where(offer => offer.Title == title && offer.Company.Name == companyName)
                .Include(offer => offer.Company)
                    .ThenInclude(company => company.User)
                .Include(offer => offer.User);
        }
    }

}