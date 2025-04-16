namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a actualiza o alocare de job.
/// </summary>
public class JobAssignmentUpdateDTO
{
    public DateTime? AssignedAt { get; set; } // Noua data a alocarii (optional)
}
