namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a actualiza o oferta de job.
/// </summary>
public class JobOfferUpdateDTO
{
    public string? Title { get; set; } // Titlu nou (optional)
    public string? Description { get; set; } // Descriere noua (optional)
    public decimal? Salary { get; set; } // Salariu nou (optional)
}
