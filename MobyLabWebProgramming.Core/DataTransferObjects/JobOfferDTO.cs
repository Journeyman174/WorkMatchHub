namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a transmite informatii despre o oferta de job.
/// </summary>
public class JobOfferDTO
{
    public Guid Id { get; set; } // Id-ul ofertei de job
    public string Title { get; set; } = null!; // Titlul ofertei
    public string Description { get; set; } = null!; // Descrierea ofertei
    public decimal Salary { get; set; } // Salariul oferit
    public CompanyDTO Company { get; set; } = null!; // Compania care ofera jobul
    public UserDTO Recruiter { get; set; } = null!; // Recruiterul care a postat jobul
    public DateTime CreatedAt { get; set; } // Data crearii ofertei
}