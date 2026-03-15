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
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClienteController(ClienteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ClienteDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtRoute("ObterClientePorId", new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, ClienteDto dto)
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

        [HttpGet("{id}", Name = "ObterClientePorId")]
        public async Task<ActionResult<ResponseDto<ClienteDto>>> GetByIdAsync(int id)
        {
            var resposta = await _service.ObterPorIdAsync(id);
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound(resposta);

            return Ok(resposta);
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDto>>> GetAllAsync()
        {
            var lista = await _service.ObterTodosAsync();
            return Ok(lista);
        }

        [HttpGet("buscar-por-ca/{caId}")]
        public async Task<ActionResult<List<ClienteDto>>> BuscarPorCaAsync(int caId)
        {
            var lista = await _service.BuscarPorCaAsync(caId);
            return Ok(lista);
        }

        [HttpPost("listar")]
        public async Task<ActionResult<List<ClienteDto>>> ListarAsync([FromBody] ClienteRequest req)
        {
            var lista = await _service.PesquisarListaAsync(req);
            return Ok(lista);
        }
    }
}
