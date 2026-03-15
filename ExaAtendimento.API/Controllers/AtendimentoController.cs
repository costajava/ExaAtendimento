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
    public class AtendimentoController : ControllerBase
    {
        private readonly AtendimentoService _service;

        public AtendimentoController(AtendimentoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(AtendimentoDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtRoute("ObterAtendimentoPorId", new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, AtendimentoDto dto)
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

        [HttpPatch("{id}")]
        public async Task<ActionResult> EncerrarAsync(int id)
        {
            await _service.EncerrarAync(id);
            return NoContent();
        }

        [HttpGet("{id}", Name = "ObterAtendimentoPorId")]
        public async Task<ActionResult<ResponseDto<AtendimentoDto>>> GetByIdAsync(int id)
        {
            var resposta = await _service.ObterPorIdAsync(id);
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound(resposta);

            return Ok(resposta);
        }


        [HttpGet]
        public async Task<ActionResult<List<AtendimentoDto>>> GetAllAsync()
        {
            var lista = await _service.ObterTodosAsync();
            return Ok(lista);
        }

        [HttpPost("listar")]
        public async Task<ActionResult<List<AtendimentoDto>>> ListarAsync([FromBody] AtendimentoRequest req)
        {
            var lista = await _service.PesquisarListaAsync(req);
            return Ok(lista);
        }

    }
}
