namespace MobyLabWebProgramming.Infrastructure.Authorization;

/// <summary>
/// This record is used to store the claims extracted from the JWT
/// </summary>
public record UserClaims(
    Guid Id,
    string? Name,
    string? Email,
    string? Role = null,
    bool IsVerified = false,
    string? FullName = null,
    string? CompanyName = null
);

