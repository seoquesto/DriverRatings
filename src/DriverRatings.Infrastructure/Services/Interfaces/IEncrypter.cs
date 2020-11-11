namespace src.DriverRatings.Infrastructure.Services.Interfaces
{
  public interface IEncrypter
  {
    string GetSalt(string value);
    string GetHash(string value, string salt);
  }
}