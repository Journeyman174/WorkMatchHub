namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a adauga o oferta de job.
/// </summary>
public class JobOfferAddDTO
{
    public string Title { get; set; } = null!; // Titlul ofertei
    public string Description { get; set; } = null!; // Descrierea ofertei
    public decimal Salary { get; set; } // Salariul oferit
}
