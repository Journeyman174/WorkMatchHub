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
    public Company Company { get; set; } = null!;

    public Guid UserId { get; set; } // UserId-ul utilizatorului cu rol Recruiter care a creat jobul
    public User User { get; set; } = null!;

    // Relații
    public ICollection<JobAssignment> JobAssignments { get; set; } = new List<JobAssignment>();
    public ICollection<JobRequest> JobRequests { get; set; } = new List<JobRequest>();
    public ICollection<SavedJob> SavedByUsers { get; set; } = new List<SavedJob>();
}

