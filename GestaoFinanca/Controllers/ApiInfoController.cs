using Microsoft.AspNetCore.Mvc;

namespace Fiap.GestaoFinanca.Api.Controllers
{

    [ApiController]
    [Route("api/info")]
    public class ApiInfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInfo()
        {
            var response = new
            {
                name ="Gestao pessoal de finanças",
                description = "API para gestão de gastos pessoais",
                versao = "1.0.0",
                Framework = ".NET 10.0"
            };

            return Ok(response);
        }
    }
}
