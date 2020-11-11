namespace src.DriverRatings.Infrastructure.Exceptions
{
  public static class UsersServiceErrorCodes
  {
    public const string EmailInUse = "email_in_use";
    public const string UsernameInUse = "username_in_use";
    public const string InvalidCredentials = "invalid_credentials";
    public const string UserDoesNotExist = "user_does_not_exist";
  }

  public static class PostsServiceErrorCodes
  {
    public const string LackOfContent = "lack_of_post_content";
    public const string UsernameInUser = "username_in_use";
    public const string InvalidCredentials = "invalid_credentials";
  }

    public static class TokenManagerErrorCodes
  {
    public const string RefreshTokenNotFound = "refresh_token_not_found";
    public const string RefreshTokenRevoked = "refresh_token_revoked";
  }
}