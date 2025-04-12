using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Entities;

public class JobOffer : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Salary { get; set; }
    public Guid CompanyId { get; set; }
    public Guid RecruiterId { get; set; }

    public Guid UserId { get; set; }

    // Relatii
    public Company Company { get; set; } = null!;
    public User Recruiter { get; set; } = null!;
    public ICollection<JobAssignment> JobAssignments { get; set; } = null!;
    public ICollection<JobRequest> JobRequests { get; set; } = null!;
    public ICollection<SavedJob> SavedByUsers { get; set; } = null!;

}
