using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to add a user, note that it doesn't have an id property because the id for the user entity should be added by the application.
/// </summary>
public class UserAddDTO
{
    public string Name { get; set; } = null!; // Username or display name.
    public string Email { get; set; } = null!; // Email address of the user.
    public string Password { get; set; } = null!; // Password in plain text.
    public UserRoleEnum Role { get; set; } // Role assigned to the user.
    public bool IsVerified { get; set; } = false; // Indicates if the user is verified (default: false).
    public string? FullName { get; set; } // Full name of the user (optional).        
}
