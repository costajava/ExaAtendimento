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
    public class TipoAtendimentoController : ControllerBase
    {
        private readonly TipoAtendimentoService _service;

        public TipoAtendimentoController(TipoAtendimentoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(TipoAtendimentoDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtRoute("ObterTipoAtendimentoPorId", new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, TipoAtendimentoDto dto)
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

        [HttpGet("{id}", Name = "ObterTipoAtendimentoPorId")]
        public async Task<ActionResult<ResponseDto<TipoAtendimentoDto>>> GetByIdAsync(int id)
        {
            var resposta = await _service.ObterPorIdAsync(id);
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound(resposta);

            return Ok(resposta);
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoAtendimentoDto>>> GetAllAsync()
        {
            var lista = await _service.ObterTodosAsync();
            return Ok(lista);
        }

        [HttpPost("listar")]
        public async Task<ActionResult<List<TipoAtendimentoDto>>> ListarAsync([FromBody] TipoAtendimentoRequest req)
        {
            var lista = await _service.PesquisarListaAsync(req);
            return Ok(lista);
        }

    }
}
