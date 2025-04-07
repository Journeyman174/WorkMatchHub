using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Entities
{
    public class JobRequest : BaseEntity
    {
        public Guid JobOfferId { get; set; }
        public Guid JobSeekerId { get; set; }
        public string CoverLetter { get; set; } = null!;

        // Relatii
        public JobOffer JobOffer { get; set; } = null!;
        public User JobSeeker { get; set; } = null!;
        public JobAssignment JobAssignment { get; set; } = null!;
    }
}
