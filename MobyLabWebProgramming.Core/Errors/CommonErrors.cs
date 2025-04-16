using System.Net;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    // General & System Errors
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);

    // Access & Permission Errors
    public static ErrorMessage Forbidden => new(HttpStatusCode.Forbidden, "You do not have permission to perform this action.", ErrorCodes.CannotAccess);
    public static ErrorMessage OnlyAdmin => new(HttpStatusCode.Forbidden, "Only Admins can verify users!", ErrorCodes.OnlyAdmin);

    // User Errors
    public static ErrorMessage InvalidLoginData => new(HttpStatusCode.BadRequest, "Login data is incomplete or invalid.", ErrorCodes.InvalidLoginData);
    public static ErrorMessage InvalidUserData => new(HttpStatusCode.BadRequest, "The provided user data is not valid.", ErrorCodes.InvalidUserData);
    public static ErrorMessage InvalidUserId => new(HttpStatusCode.BadRequest, "The provided user ID is invalid.", ErrorCodes.InvalidUserId);
    public static ErrorMessage UserAlreadyExists => new(HttpStatusCode.Conflict, "The user already exists!", ErrorCodes.UserAlreadyExists);
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage WrongPassword => new(HttpStatusCode.BadRequest, "The password is incorrect.", ErrorCodes.WrongPassword);

    // Company Errors
    public static ErrorMessage CompanyAlreadyExists => new(HttpStatusCode.Conflict, "Company already exists!", ErrorCodes.CompanyAlreadyExists);
    public static ErrorMessage CompanyNotFound => new(HttpStatusCode.NotFound, "Company doesn't exist!", ErrorCodes.CompanyNotFound);
    public static ErrorMessage InvalidCompanyData => new(HttpStatusCode.BadRequest, "The provided company data is not valid.", ErrorCodes.InvalidCompanyData);
    public static ErrorMessage RecruiterCompanyExists => new(HttpStatusCode.Conflict, "Recruiter already owns a company!", ErrorCodes.RecruiterCompanyExists);
    public static ErrorMessage MultipleCompaniesFound => new(HttpStatusCode.Conflict, "User is associated with multiple companies!", ErrorCodes.MultipleCompaniesFound);


    // Job Offer Errors
    public static ErrorMessage CannotSaveOwnJob => new(HttpStatusCode.Forbidden, "You cannot save your own job offer.", ErrorCodes.CannotSaveOwnJob);
    public static ErrorMessage InvalidJobOfferData => new(HttpStatusCode.BadRequest, "Invalid job offer data provided.", ErrorCodes.InvalidJobOfferData);
    public static ErrorMessage JobAlreadySaved => new(HttpStatusCode.Conflict, "Job is already saved!", ErrorCodes.JobAlreadySaved);
    public static ErrorMessage JobOfferNotFound => new(HttpStatusCode.NotFound, "Job offer not found.", ErrorCodes.JobOfferNotFound);

    // Job Request Errors
    public static ErrorMessage InvalidJobRequestData => new(HttpStatusCode.BadRequest, "Invalid job request data.", ErrorCodes.InvalidJobRequestData);
    public static ErrorMessage JobRequestAlreadyExists => new(HttpStatusCode.Conflict, "Job request already exists!", ErrorCodes.JobRequestAlreadyExists);
    public static ErrorMessage JobRequestNotFound => new(HttpStatusCode.NotFound, "Job request not found.", ErrorCodes.JobRequestNotFound);
    public static ErrorMessage MultipleJobRequestsFound => new(HttpStatusCode.Conflict, "Multiple job requests found for the current user.", ErrorCodes.MultipleJobRequestsFound);

    // Job Assignment Errors
    public static ErrorMessage InvalidJobAssignmentData => new(HttpStatusCode.BadRequest, "Invalid job assignment data provided.", ErrorCodes.InvalidJobAssignmentData);
    public static ErrorMessage JobAssignmentAlreadyExists => new(HttpStatusCode.Conflict, "Job Assignment already exists!", ErrorCodes.JobAssignmentAlreadyExists);
    public static ErrorMessage JobAssignmentNotFound => new(HttpStatusCode.NotFound, "Job assignment not found.", ErrorCodes.JobAssignmentNotFound);

    // Saved Job Errors
    public static ErrorMessage SavedJobNotFound => new(HttpStatusCode.NotFound, "Saved job not found.", ErrorCodes.SavedJobNotFound);

    // Validation Errors
    public static ErrorMessage InvalidId => new(HttpStatusCode.BadRequest, "The provided ID is invalid.", ErrorCodes.InvalidId);
    public static ErrorMessage InvalidPaginationParams => new(HttpStatusCode.BadRequest, "Invalid pagination parameters provided.", ErrorCodes.InvalidPaginationParams);
}