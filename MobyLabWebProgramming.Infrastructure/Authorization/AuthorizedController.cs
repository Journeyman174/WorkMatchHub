using System.Security.Claims;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Handlers;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Authorization;

/// <summary>
/// This abstract class is used as a base class for controllers that need to get current information about the user from the database.
/// </summary>
public abstract class AuthorizedController(IUserService userService) : BaseResponseController
{
    private UserClaims? _userClaims;
    protected readonly IUserService UserService = userService;

    /// <summary>
    /// This method extracts the claims from the JWT into an object.
    /// It also caches the object if used a second time.
    /// </summary>
    protected UserClaims ExtractClaims()
    {
        if (_userClaims != null)
        {
            return _userClaims;
        }

        var claimsList = User.Claims.ToList();

        var userIdClaim = claimsList.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid or missing user ID in claims.");
        }

        var email = claimsList.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var name = claimsList.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        // Extended claims
        var role = claimsList.FirstOrDefault(x => x.Type == "UserRole")?.Value;
        var companyName = claimsList.FirstOrDefault(x => x.Type == "CompanyName")?.Value;
        var fullName = claimsList.FirstOrDefault(x => x.Type == "FullName")?.Value;

        var isVerified = claimsList
            .Where(x => x.Type == "IsVerified")
            .Select(x => bool.TryParse(x.Value, out var val) && val)
            .FirstOrDefault();

        _userClaims = new(userId, name, email, role, isVerified, fullName, companyName);

        return _userClaims;
    }

    /// <summary>
    /// This method also gets the currently logged user information from the database to provide more information to authorization verifications.
    /// </summary>
    protected Task<ServiceResponse<UserDTO>> GetCurrentUser() => UserService.GetUser(ExtractClaims().Id);
}