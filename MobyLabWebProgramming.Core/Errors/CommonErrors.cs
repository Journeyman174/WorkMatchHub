using System.Net;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);

    public static ErrorMessage CompanyNotFound => new(HttpStatusCode.NotFound, "Company doesn't exist!", ErrorCodes.CompanyNotFound);
    public static ErrorMessage CompanyAlreadyExists => new(HttpStatusCode.Conflict, "Company already exists!", ErrorCodes.CompanyAlreadyExists);
    public static ErrorMessage Forbidden => new(HttpStatusCode.Forbidden, "You do not have permission to perform this action.", ErrorCodes.CannotAccess);

    public static ErrorMessage JobAlreadySaved => new(HttpStatusCode.Conflict, "Job is already saved!", ErrorCodes.JobAlreadySaved);

    public static ErrorMessage SavedJobNotFound => new(HttpStatusCode.NotFound, "Saved job not found.", ErrorCodes.SavedJobNotFound);

    public static ErrorMessage JobOfferNotFound => new(HttpStatusCode.NotFound, "Job offer not found.", ErrorCodes.JobOfferNotFound);

    public static ErrorMessage RecruiterCompanyExists => new(HttpStatusCode.Conflict, "Recruiter already owns a company!", ErrorCodes.RecruiterCompanyExists);

    public static ErrorMessage JobRequestNotFound => new(HttpStatusCode.NotFound, "Job request not found.", ErrorCodes.JobRequestNotFound);

    public static ErrorMessage OnlyAdmin => new(HttpStatusCode.Forbidden, "Only Admins can verify users!", ErrorCodes.OnlyAdmin);


    





}

