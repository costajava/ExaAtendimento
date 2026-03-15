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
    public class ModuloController : ControllerBase
    {
        private readonly ModuloService _service;

        public ModuloController(ModuloService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ModuloDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtRoute("ObterModuloPorId", new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, ModuloDto dto)
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

        [HttpGet]
        public async Task<ActionResult<List<ModuloDto>>> GetAllAsync()
        {
            var lista = await _service.ObterTodosAsync();
            return Ok(lista);
        }

        [HttpGet("{id}", Name = "ObterModuloPorId")]
        public async Task<ActionResult<ResponseDto<ModuloDto>>> GetByIdAsync(int id)
        {
            var resposta = await _service.ObterPorIdAsync(id);
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound(resposta);

            return Ok(resposta);
        }

        [HttpPost("listar")]
        public async Task<ActionResult<PagedListDto<ModuloDto>>> ListarAsync([FromBody] ModuloRequest req)
        {
            var lista = await _service.PesquisarListaAsync(req);
            return Ok(lista);
        }
    }
}

