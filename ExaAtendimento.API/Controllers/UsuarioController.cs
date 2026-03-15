using ExaAtendimento.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Application.Services;

namespace ExaAtendimento.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(UsuarioCriacaoDto dto)
        {
            await _service.AdicionarAsync(dto);
            return CreatedAtRoute("ObterUsuarioPorId", new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, UsuarioDto dto)
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

        [HttpGet("{id}", Name = "ObterUsuarioPorId")]
        public async Task<ActionResult<ResponseDto<UsuarioDto>>> GetByIdAsync(int id)
        {
            var resposta = await _service.ObterPorIdAsync(id);
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound(resposta);

            return Ok(resposta);
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> GetAllAsync()
        {
            var lista = await _service.ObterTodosAsync();
            return Ok(lista);
        }

        [HttpPost("listar")]
        public async Task<ActionResult<List<UsuarioDto>>> ListarAsync([FromBody] UsuarioRequest req)
        {
            var lista = await _service.PesquisarListaAsync(req);
            return Ok(lista);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<object>>> LoginAsync([FromBody] UsuarioLoginDto loginDto,
                                                           [FromServices] JwtTokenService tokenService)
        {
            var resposta = await _service.LoginAsync(loginDto);
            if (!resposta.Sucesso || resposta.Dados == null)
                return Unauthorized(resposta);

            var token = tokenService.GerarToken(resposta.Dados);

            var resultado = new
            {
                token,
                usuario = resposta.Dados
            };

            return Ok(new ResponseDto<object>(resultado, "Login realizado com sucesso.", true));
        }

        [HttpPost("trocar-senha")]
        public async Task<IActionResult> TrocarSenhaAsync(TrocarSenhaDto dto)
        {
            await _service.TrocarSenhaAsync(dto);
            return Ok(new { mensagem = "Senha alterada com sucesso." });
        }

        [HttpPost("esqueci-senha")]
        public async Task<IActionResult> EsqueciSenhaAsync(string email)
        {
            await _service.SolicitarRedefinirSenhaAsync(email);
            return Ok(new { mensagem = "Instruções para redefinição de senha enviadas no seu e-mail ." });
        }
        
        [HttpPost("validar-codigo-seguranca")]
        public async Task<IActionResult> ValidarCodigoSegurancaAsync(string email,string codigoSeguranca)
        {
            await _service.ValidarCodigoSegurancaAsync(email, codigoSeguranca);
            return Ok(new { mensagem = "Código de segurança válidado com sucesso." });
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenhaAsync(RedefinirSenhaDto dto)
        {
            await _service.RedefinirSenhaAsync(dto);
            return Ok(new { mensagem = "Senha redefinida com sucesso." });
        }

    }
}
