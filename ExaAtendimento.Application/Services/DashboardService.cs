using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Interfaces;

namespace ExaAtendimento.Application.Services
{
    public class DashboardService
    {
        private readonly AssuntoService _assuntoService;
        private readonly AtendimentoService _atendimentoService;
        private readonly CaService _caService;
        private readonly ClienteService _clienteService;
        private readonly ModuloService _moduloService;
        private readonly SugestaoService _sugestaoService;
        private readonly TipoAtendimentoService _tipoAtendimentoService;
        private readonly UsuarioService _usuarioService;

        public DashboardService(AssuntoService assuntoService,
                                AtendimentoService atendimentoService, 
                                CaService caService,
                                ClienteService clienteService,
                                ModuloService moduloService,
                                SugestaoService sugestaoService,
                                TipoAtendimentoService tipoAtendimentoService,
                                UsuarioService usuarioService) 
        { 
            _assuntoService = assuntoService;
            _atendimentoService = atendimentoService;
            _caService = caService; 
            _clienteService = clienteService;
            _moduloService = moduloService;
            _sugestaoService = sugestaoService;
            _tipoAtendimentoService = tipoAtendimentoService;
            _usuarioService = usuarioService;
        }   

        public async Task<DashboardDto> GetDadosAsync()
        {   var dto = new DashboardDto();
            dto.TotalAssunto = await _assuntoService.GetTotalCountAsync();
            dto.TotalAtendimento = await _atendimentoService.GetTotalCountAsync();
            dto.TotalCa = await _caService.GetTotalCountAsync();
            dto.TotalCliente = await _clienteService.GetTotalCountAsync();
            dto.TotalModulo = await _moduloService.GetTotalCountAsync();
            dto.TotalSugestao = await _sugestaoService.GetTotalCountAsync();
            dto.TotalTipoAtendimento = await _tipoAtendimentoService.GetTotalCountAsync();
            dto.TotalUsuario = await _usuarioService.GetTotalCountAsync();
            return (dto);
        }
    }
}
