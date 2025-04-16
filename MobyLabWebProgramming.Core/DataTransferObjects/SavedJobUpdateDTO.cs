namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a actualiza informatii despre un job salvat.
/// </summary>
public class SavedJobUpdateDTO
{
    public JobOfferDTO? JobOffer { get; set; } // Oferta de job noua (optional)
}
