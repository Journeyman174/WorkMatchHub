using System.Net;
using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

/// <summary>
/// Inject the required services through the constructor.
/// </summary>
public class UserService(IRepository<WebAppDatabaseContext> repository, ILoginService loginService, IMailService mailService)
    : IUserService
{
    public async Task<ServiceResponse<UserDTO>> GetUser(Guid id, CancellationToken cancellationToken = default)
    {
        // Validate the user ID
        if (id == Guid.Empty)
        {
            return ServiceResponse.FromError<UserDTO>(CommonErrors.InvalidUserId);
        }

        // Get a user using a specification on the repository
        var result = await repository.GetAsync(new UserProjectionSpec(id), cancellationToken);

        // Pack the result or error into a ServiceResponse
        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<UserDTO>(CommonErrors.UserNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<UserDTO>>> GetUsers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        // Check if pagination parameters are missing
        if (pagination == null)
        {
            return ServiceResponse.FromError<PagedResponse<UserDTO>>(CommonErrors.InvalidPaginationParams);
        }

        var result = await repository.PageAsync(pagination, new UserProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<LoginResponseDTO>> Login(LoginDTO login, CancellationToken cancellationToken = default)
    {
        if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
        {
            return ServiceResponse.FromError<LoginResponseDTO>(CommonErrors.InvalidLoginData);
        }

        var entity = await repository.GetAsync(new UserSpec(login.Email), cancellationToken); // Retrieve the raw entity for validation.

        if (entity == null) // Check if user exists.
        {
            return ServiceResponse.FromError<LoginResponseDTO>(CommonErrors.UserNotFound);
        }

        if (entity.Password != login.Password) // Validate password.
        {
            return ServiceResponse.FromError<LoginResponseDTO>(CommonErrors.WrongPassword);
        }

        var user = await repository.GetAsync(new UserProjectionSpec(entity.Id), cancellationToken); // Get the safe DTO version.

        if (user == null)
        {
            return ServiceResponse.FromError<LoginResponseDTO>(CommonErrors.UserNotFound);
        }

        return ServiceResponse.ForSuccess(new LoginResponseDTO
        {
            User = user,
            Token = loginService.GetToken(user, DateTime.UtcNow, new TimeSpan(7, 0, 0, 0))
        });
    }

    public async Task<ServiceResponse<int>> GetUserCount(CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await repository.GetCountAsync<User>(cancellationToken); // Get total user count from the database.
            return ServiceResponse.ForSuccess(count);
        }
        catch (Exception)
        {
            return ServiceResponse.FromError<int>(CommonErrors.TechnicalSupport); // Return a generic error if something goes wrong.
        }
    }

    public async Task<ServiceResponse> AddUser(UserAddDTO user, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        // Validate user input
        if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
        {
            return ServiceResponse.FromError(CommonErrors.InvalidUserData);
        }

        // Verify if the requesting user has the right to add a user
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Check if a user with the same email already exists
        var result = await repository.GetAsync(new UserSpec(user.Email), cancellationToken);
        if (result != null)
        {
            return ServiceResponse.FromError(CommonErrors.UserAlreadyExists);
        }

        // Create and persist the new user entity
        await repository.AddAsync(new User
        {
            Email = user.Email,
            Name = user.Name,
            Role = user.Role,
            Password = user.Password,
            IsVerified = requestingUser?.Role == UserRoleEnum.Admin
                ? user.IsVerified
                : user.Role is UserRoleEnum.Admin or UserRoleEnum.Recruiter,
            FullName = user.FullName
        }, cancellationToken);

        // Send a welcome email
        await mailService.SendMail(
            user.Email,
            "Welcome!",
            MailTemplates.UserAddTemplate(user.FullName ?? user.Name),
            true,
            "WorkMatchHub",
            cancellationToken
        );

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateUser(UserUpdateDTO user, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        // Validate input data
        if (user == null || user.Id == Guid.Empty)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidUserData);
        }

        // Check if the user is allowed to update the user info
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != user.Id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Retrieve the existing user entity
        var entity = await repository.GetAsync(new UserSpec(user.Id), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.UserNotFound);
        }

        // Update user fields with provided values
        entity.Name = user.Name ?? entity.Name;
        entity.Password = user.Password ?? entity.Password;

        await repository.UpdateAsync(entity, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteUser(Guid id, UserDTO? requestingUser = null, CancellationToken cancellationToken = default)
    {
        // Validate the provided user ID
        if (id == Guid.Empty)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidUserId);
        }

        // Verify if the requesting user has permission to delete the user
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != id)
        {
            return ServiceResponse.FromError(CommonErrors.Forbidden);
        }

        // Check if the user exists before attempting deletion
        var user = await repository.GetAsync(new UserSpec(id), cancellationToken);
        if (user == null)
        {
            return ServiceResponse.FromError(CommonErrors.UserNotFound);
        }

        await repository.DeleteAsync<User>(id, cancellationToken); // Delete the user entity

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> VerifyUser(Guid userId, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        // Check if the requesting user has admin privileges
        if (requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(CommonErrors.OnlyAdmin);
        }

        // Validate the provided user Id
        if (userId == Guid.Empty)
        {
            return ServiceResponse.FromError(CommonErrors.InvalidUserId);
        }

        // Retrieve the user to be verified
        var user = await repository.GetAsync(new UserSpec(userId), cancellationToken);
        if (user == null)
        {
            return ServiceResponse.FromError(CommonErrors.UserNotFound);
        }

        user.IsVerified = true; // Set the user's verified status to true
        await repository.UpdateAsync(user, cancellationToken); // Persist the changes

        return ServiceResponse.ForSuccess();
    }

}