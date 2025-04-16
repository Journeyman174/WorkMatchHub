namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to update a user, the properties besides the id are nullable to indicate that they may not be updated if they are null.
/// </summary>
public record UserUpdateDTO(
    Guid Id, // User's unique identifier.
    string? Name = null, // Optional update for username.
    string? Password = null, // Optional update for password.
    string? FullName = null, // Optional update for full name.
    bool? IsVerified = null // Optional update for verification status.   
);
