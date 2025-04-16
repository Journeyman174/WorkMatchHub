using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user within the application and to client application.
/// Note that it doesn't contain a password property and that is why you should use DTO rather than entities to use only the data that you need or protect sensible information.
/// </summary>
public class UserDTO
{
    public Guid Id { get; set; } // Unique identifier for the user.
    public string Name { get; set; } = null!; // Username or display name.
    public string Email { get; set; } = null!; // Email address of the user.
    public UserRoleEnum Role { get; set; } // Role of the user (Admin, Recruiter, JobSeeker).
    public bool IsVerified { get; set; } // Indicates if the user is verified.
    public string? FullName { get; set; } // Full name of the user (optional).
    public string? CompanyName { get; set; } // For Recruiters, displays the associated company name.
}
