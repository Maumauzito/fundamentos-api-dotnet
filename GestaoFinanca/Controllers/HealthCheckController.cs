using Microsoft.AspNetCore.Mvc;

namespace Fiap.GestaoFinanca.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() 
        {
            return Ok(new
            { 
                status = "API em execução",
                application = "Gestão Financeira",
                Framework = ".NET 10.0"
            });
        }
    }
}
