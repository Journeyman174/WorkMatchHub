using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public Guid UserId { get; set; }

    // Relatia One-to-Many cu JobOffer
    public ICollection<JobOffer> JobOffers { get; set; } = null!;
}
