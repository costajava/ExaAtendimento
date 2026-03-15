using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaAtendimento.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _service;
        public DashboardController(DashboardService service) 
        { 
            _service = service; 
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDadosAsync()
        {
            var dados = await _service.GetDadosAsync();
            return Ok(dados);
        }

    }
}
