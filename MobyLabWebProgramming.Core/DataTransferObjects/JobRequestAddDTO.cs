using System;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class JobRequestAddDTO
{
    public Guid JobOfferId { get; set; }
    public string CoverLetter { get; set; } = null!;
}
