using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class User : BaseEntity
{
    // Numele utilizatorului - camp obligatoriu
    public string Name { get; set; } = null!;

    // Adresa de email a utilizatorului - camp obligatoriu
    public string Email { get; set; } = null!;

    // Parola utilizatorului - camp obligatoriu
    public string Password { get; set; } = null!;

    // Rolul utilizatorului - definit in UserRoleEnum (Admin, Recruiter, JobSeeker)
    public UserRoleEnum Role { get; set; }

    // Relatie One-to-One: un utilizator (cu rol Recruiter) poate detine o companie
    public Company? Company { get; set; }

    // Daca utilizatoruli i-a fost verificat contul
    public bool IsVerified { get; set; } = false;

    // Numele complet al utilizatorului - optional
    public string? FullName { get; set; }

    /// <summary>
    /// References to other entities such as this are used to automatically fetch correlated data, this is called a navigation property.
    /// Collection such as this can be used for Many-To-One or Many-To-Many relations.
    /// Note that this field will be null if not explicitly requested via a Include query, also note that the property is used by the ORM, in the database this collection doesn't exist. 
    /// </summary>
    public ICollection<UserFile> UserFiles { get; set; } = null!;

    // Relatie One-to-Many: un utilizator poate trimite mai multe cereri de job
    public ICollection<JobRequest> JobRequests { get; set; } = null!;

    // Relatie One-to-Many: un utilizator (Recruiter) poate crea mai multe oferte de job
    public ICollection<JobOffer> JobOffers { get; set; } = null!;

    // Relatie Many-to-Many (prin SavedJob): utilizatorul poate salva mai multe joburi
    public ICollection<SavedJob> SavedJobs { get; set; } = null!;
}