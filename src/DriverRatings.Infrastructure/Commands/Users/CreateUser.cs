namespace src.DriverRatings.Infrastructure.Commands.Users
{
  public class CreateUser : AuthenticateCommandBase
  {
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}