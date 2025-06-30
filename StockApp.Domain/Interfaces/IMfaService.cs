namespace StockApp.Domain.Interfaces
{
    public interface IMfaService
    {
        string GenerateOtp();
        bool ValidateOtp(string userOtp, string storedOtp);
    }
}
