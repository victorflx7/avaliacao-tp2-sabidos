using StockApp.Domain.Interfaces;

namespace StockApp.Application.Services
{
    public class MfaService : IMfaService
    {
        public string GenerateOtp()
        {
            var otp = new Random().Next(100000, 999999).ToString();
            return otp;
        }
        public bool ValidateOtp(string userOtp, string storedOtp)
        {
            return userOtp == storedOtp;
        }
    }
}
