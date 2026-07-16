using Microsoft.AspNetCore.Mvc;
using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Domain.Entities;
using Fiap.GestaoFinanca.Application.Mappings;
using Fiap.GestaoFinanca.Application.Interfaces;

namespace Fiap.GestaoFinanca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DespesasController : ControllerBase
    {

        private readonly IDespesaService _despesaService;

        public DespesasController(IDespesaService despesaService)
        {
            _despesaService = despesaService;
        }



        [HttpGet("exemplo")]
        public IActionResult ObterExemplo()
        {
            var despesa = new Despesa(
                "Aluguel",
                1500.00m,
                DateTime.Now,
                "Moradia",
                "Boleto Bancário"
                );

            var response = despesa.ToResponse();
            
            return Ok(response);
        }

        
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] DespesaRequest request)
        {

            var response = await _despesaService.CriarAsync(request);

            return Created($"api/despesas/{response.Id}", response);

        }

    }
}
