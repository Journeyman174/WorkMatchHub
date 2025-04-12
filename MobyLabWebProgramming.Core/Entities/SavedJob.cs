using System;

namespace MobyLabWebProgramming.Core.Entities
{
    public class SavedJob : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Guid JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; } = default!;
    }
}
