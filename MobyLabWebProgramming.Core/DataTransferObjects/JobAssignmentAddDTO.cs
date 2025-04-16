namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO folosit pentru a crea o atribuire de job. Identificarea se face pe baza titlului jobului si a emailului utilizatorului.
/// </summary>
public class JobAssignmentAddDTO
{
    public string JobTitle { get; set; } = null!; // Titlul jobului pentru care se face atribuirea
    public string JobSeekerEmail { get; set; } = null!; // Emailul utilizatorului caruia i se atribuie jobul
}
