using MobyLabWebProgramming.Core.DataTransferObjects;
using System;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class JobRequestDTO
{
    public Guid Id { get; set; }
    public JobOfferDTO JobOffer { get; set; } = null!;
    public UserDTO User { get; set; } = null!; 
    public string CoverLetter { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
