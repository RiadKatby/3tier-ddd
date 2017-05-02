
namespace RefactorName.Core
{
    /// <summary>
    /// first number is for error source(domain, repository ..etc)
    /// next two digits number is unique for the error source(ex. 01 for sql repository)
    /// last two digits number is a sequence inside the sub-source.
    /// </summary>
    public enum ErrorCode
    {
        NotAuthorized = 20101,        
        LoginFailed = 20102,

        //sql server repository
        ModifiedbyAnotherUserCheckUpdates = 50101,
        DatabaseError = 50102,
        DatabaseInvalidOperation = 50103,
        DatabaseInvalidData = 50104,

        //other repositories
        ActiveDirectoryNotAvailable = 50201,
        ActiveDirectoryUserDisabled = 50202,


        //domain error codes
        GreaterThanZero = 60101,
        InvalidData = 60102,
        NotFound = 60103,
        NotUnique = 60104,

        //identity Errors
        IdentityUserCreateError = 60201,

    }
}
