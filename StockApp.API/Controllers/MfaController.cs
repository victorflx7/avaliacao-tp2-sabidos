using Microsoft.AspNetCore.Mvc;
using StockApp.Domain.Interfaces;

namespace StockApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MfaController : ControllerBase
    {
        private readonly IMfaService _mfaService;
        public MfaController(IMfaService mfaService)
        {
            _mfaService = mfaService;
        }

        [HttpGet("generate")]
        public IActionResult GenerateOtp()
        {
            var otp = _mfaService.GenerateOtp();
            return Ok(new { otp });
        }
    }
}
