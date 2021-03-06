using src.DriverRatings.Server.Core.Exceptions;

namespace src.DriverRatings.Server.Core.Models
{
  public class PlateIdentifier
  {
    public string Identifier { get; protected set; }

    protected PlateIdentifier()
    {
    }

    public PlateIdentifier(string identifier)
    {
      string fixedIdentifier = identifier?.Trim().ToUpperInvariant();
      if (string.IsNullOrEmpty(fixedIdentifier))
      {
        throw new InvalidPlateIdentifierException(fixedIdentifier);
      }

      if (this.Identifier == fixedIdentifier)
      {
        return;
      }

      this.Identifier = fixedIdentifier;
    }
  }
}