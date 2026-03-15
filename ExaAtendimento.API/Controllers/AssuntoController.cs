using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Interfaces;
using ExaAtendimento.Application.Services;
using ExaAtendimento.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaAtendimento.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssuntoController : ControllerBase
    {
        private readonly AssuntoService _service;

        public AssuntoController(AssuntoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(AssuntoDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtRoute("ObterAssuntoPorId", new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, AssuntoDto dto)
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

        [HttpGet("{id}", Name = "ObterAssuntoPorId")]
        public async Task<ActionResult<ResponseDto<AssuntoDto>>> GetByIdAsync(int id)
        {
            var resposta = await _service.ObterPorIdAsync(id);
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound(resposta);

            return Ok(resposta);
        }

        [HttpGet]
        public async Task<ActionResult<List<AssuntoDto>>> GetAllAsync()
        {
            var lista = await _service.ObterTodosAsync();
            return Ok(lista);
        }

        [HttpPost("listar")]
        public async Task<ActionResult<List<CaDto>>> ListarAsync([FromBody] AssuntoRequest req)
        {
            var lista = await _service.PesquisarListaAsync(req);
            return Ok(lista);
        }
    }
}
