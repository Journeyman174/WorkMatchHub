using System.Text.Json.Serialization;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// This enumeration represents codes for common errors and should be used to better identify the error by the client. You may add or remove codes as you see fit.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCodes
{
    // General
    Unknown,
    TechnicalError,
    PhysicalFileNotFound,

    // Permissions
    CannotAccess,
    CannotAdd,
    CannotUpdate,
    CannotDelete,
    OnlyAdmin,

    // Validation
    InvalidId,
    InvalidPaginationParams,
    InvalidLoginData,
    InvalidUserId,
    InvalidUserData,
    InvalidCompanyData,
    InvalidJobAssignmentData,
    InvalidJobOfferData,
    InvalidJobRequestData,

    // User
    UserAlreadyExists,
    EntityNotFound,
    WrongPassword,

    // Company
    CompanyAlreadyExists,
    CompanyNotFound,
    RecruiterCompanyExists,
    MultipleCompaniesFound,

    // Job Offer
    CannotSaveOwnJob,
    JobAlreadySaved,
    JobOfferNotFound,

    // Job Request
    JobRequestAlreadyExists,
    JobRequestNotFound,
    MultipleJobRequestsFound,

    // Job Assignment
    JobAssignmentAlreadyExists,
    JobAssignmentNotFound,

    // Saved Job
    SavedJobNotFound,

    // Mail
    MailSendFailed
}