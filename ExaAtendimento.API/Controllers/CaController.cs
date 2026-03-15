using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Services;
using ExaAtendimento.Domain.Queries;

namespace ExaAtendimento.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CaController : ControllerBase
    {
        private readonly CaService _service;

        public CaController(CaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CaDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtRoute("ObterCaPorId", new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, CaDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Chave inconsistente.");

            await _service.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }

        [HttpGet("{id}", Name = "ObterCaPorId")]
        public async Task<ActionResult<ResponseDto<CaDto>>> GetByIdAsync(int id)
        {
            var resposta = await _service.ObterPorIdAsync(id);
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound(resposta);

            return Ok(resposta);

        }

        [HttpGet]
        public async Task<ActionResult<List<CaDto>>> GetAllAsync()
        {
            var lista = await _service.ObterTodosAsync();
            return Ok(lista);
        }

        [HttpPost("listar")]
        public async Task<ActionResult<List<CaDto>>> ListarAsync([FromBody] CaRequest req)
        {
            var lista = await _service.PesquisarListaAsync(req);
            return Ok(lista);
        }

        [HttpGet("por-nome/{nome}")]
        public async Task<ActionResult<List<CaDto>>> BuscarPorNomeAsync(string nome)
        {
            var lista = await _service.BuscarPorNomeAsync(nome);
            return Ok(lista);
        }

    }
}
