using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specificații pentru entitatea JobOffer.
/// </summary>
public sealed class JobOfferSpec : Specification<JobOffer>
{
    public JobOfferSpec(Guid id)
    {
        Query
            .Where(offer => offer.Id == id)
            .Include(offer => offer.Company)
                .ThenInclude(company => company.User)
            .Include(offer => offer.User);
    }

    public JobOfferSpec(string title)
    {
        Query.Where(offer => offer.Title == title);
    }

    public JobOfferSpec(Guid recruiterId, bool isByRecruiter)
    {
        Query
            .Where(offer => offer.UserId == recruiterId)
            .Include(offer => offer.Company)
                .ThenInclude(company => company.User)
            .Include(offer => offer.User);
    }



}
