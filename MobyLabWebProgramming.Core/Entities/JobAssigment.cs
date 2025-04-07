using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Entities;

public class JobAssignment : BaseEntity
{
    public Guid JobRequestId { get; set; }
    public Guid JobOfferId { get; set; }
    public DateTime AssignedAt { get; set; }

    public Guid UserId { get; set; }

    // Relatii
    public JobRequest JobRequest { get; set; } = null!;
    public JobOffer JobOffer { get; set; } = null!;
}
